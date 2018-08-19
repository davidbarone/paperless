using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using paperless_api.Models;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Dapper;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;

namespace paperless_api.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaperlessController : ControllerBase
    {
        public PaperlessController()
        {
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        public string ConnectionString => "Server=localhost;Database=Paperless;Trusted_Connection=True;MultipleActiveResultSets=true";

        #region Private / Help Methods

        private IEnumerable<Form> _GetForms()
        {
            var sql = "SELECT * FROM FORM";

            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Form>(sql).ToList();
            }
        }

        private Section _GetSection(int sectionId)
        {
            var sql = "SELECT * FROM SECTION WHERE ID = @sectionId";
            using (var db = new SqlConnection(ConnectionString))
            {
                var section = db.Query<Section>(sql, new { sectionId = sectionId }).First();
                return section;
            }
        }

        // Gets individual pack object
        private Pack _GetPack(int packId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Pack>(
                    "SELECT * FROM PACK WHERE ID = @packId",
                    new { packId = packId }).First();
            }
        }

        private IEnumerable<Answer> _GetQuestionAnswers(int packId, int questionId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<Answer>(
                    "SELECT * FROM ANSWER WHERE QUESTION_ID = @questionId AND PACK_ID = @packId",
                    new { packId = packId, questionId = questionId }
                );
            }
        }

        #endregion

        #region Form

        // GET api/Paperless/GetForms
        [HttpGet("GetForms")]
        public ActionResult<IEnumerable<Form>> GetForms()
        {
            return Ok(_GetForms());
        }

        // GET api/Paperless/GetPackForms
        [HttpGet("GetPackForms/{packId}")]
        public ActionResult<IEnumerable<Form>> GetPackForms(int packId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return Ok(db.Query<Form>(
                    "SELECT * FROM FORM WHERE ID IN (SELECT FORM_ID FROM PACK_FORM WHERE PACK_ID = @packId)",
                    new { packId = packId }));
            }
        }

        // POST api/Paperless/CreateForm
        [HttpPost("CreateForm")]
        public ActionResult CreateForm(Form form)
        {
            var sql = @"
            DECLARE @NEXT INT;
            SELECT @NEXT = NEXT VALUE FOR SEQ_FORM;

            INSERT INTO FORM (
                ID, NAME, DESCRIPTION, AUTHORISED_BY, CREATED_BY, VERSION, CREATED_DT, UPDATED_BY, UPDATED_DT, EFFECTIVE_DT, EXPIRY_DT
            ) VALUES (
                @NEXT, @name, @description, @authorisedBy, @createdBy, 1, GETDATE(), @createdBy, GETDATE(), @effectiveDt, '31-DEC-9999');
                
            SELECT * FROM FORM WHERE ID = @NEXT;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newForm = db.Query<Form>(sql, form).First();
                return Ok(newForm);
            }
        }

        // POST api/Paperless/CreateForm
        [HttpPost("UpdateForm")]
        public ActionResult UpdateForm(Form form)
        {
            var sql = @"
            UPDATE FORM SET
                NAME = @name,
                DESCRIPTION = @description
            WHERE
                ID = @id;
            SELECT * FROM FORM WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newForm = db.Query<Form>(sql, form).First();
                return Ok(newForm);
            }
        }

        // POST api/Paperless/CreateForm
        [HttpPost("DeleteForm/{id}")]
        public ActionResult DeleteForm(int id)
        {
            var sql = @"DELETE FROM FORM WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                db.Execute(sql, new {id = id});
                return Ok();
            }
        }

        #endregion

        #region Section

        // GET api/Paperless/GetSections
        [HttpGet("GetSections/{formId}")]
        public ActionResult<IEnumerable<Form>> GetSections(int formId)
        {
            var sql = "SELECT * FROM SECTION WHERE FORM_ID = @formId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var sections = db.Query<Section>(sql, new
                {
                    formId = formId
                }).ToList();
                return Ok(sections);
            }
        }

        // GET api/Paperless/CreateSections
        [HttpPost("CreateSection")]
        public ActionResult<IEnumerable<Section>> CreateSection(Section section)
        {
            var sql = @"
            INSERT INTO SECTION (ID, NAME, FORM_ID, TABULAR_FLAG)
            SELECT NEXT VALUE FOR SEQ_SECTION, @name, @formId, @tabularFlag;
            SELECT * FROM SECTION WHERE FORM_ID = @formId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var sections = db.Query<Section>(sql, section);
                return Ok(sections);
            }
        }

        // POST api/Paperless/UpdateSection
        [HttpPost("UpdateSection")]
        public ActionResult UpdateSection(Section section)
        {
            var sql = @"
            UPDATE SECTION SET
                NAME = @name,
                TABULAR_FLAG = @tabularFlag
            WHERE
                ID = @id;
            SELECT * FROM SECTION WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newSection = db.Query<Section>(sql, section).First();
                return Ok(newSection);
            }
        }

        // POST api/Paperless/DeleteSection
        [HttpPost("DeleteSection/{id}")]
        public ActionResult DeleteSection(int id)
        {
            var sql = @"DELETE FROM SECTION WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                db.Execute(sql, new {id = id});
                return Ok();
            }
        }

        #endregion

        #region Question

        // GET api/Paperless/GetQuestions
        [HttpGet("GetQuestions/{sectionId}")]
        public ActionResult<IEnumerable<Question>> GetQuestions(int sectionId)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return Ok(db.Query<Question>(
                    "SELECT * FROM QUESTION WHERE SECTION_ID = @sectionId",
                    new { sectionId = sectionId }));
            }
        }

        // GET api/Paperless/CreateForm
        [HttpPost("CreateQuestion")]
        public ActionResult CreateQuestion(Question question)
        {
            var sql = @"
            DECLARE @NEXT INT
            SELECT @NEXT = NEXT VALUE FOR SEQ_QUESTION;

            INSERT INTO QUESTION (
                ID, SECTION_ID, QUESTION_TYPE, LIST_NAME, PROMPT, TARGET_NAME, EFFECTIVE_DT, EXPIRY_DT
            ) VALUES (
                @NEXT, @sectionId, @questionType, @listName, @prompt, @targetName, @effectiveDt, '31-DEC-9999'
            );

            SELECT * FROM QUESTION WHERE ID = @NEXT;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newQuestion = db.Query<Question>(sql, question).First();
                return Ok(newQuestion);
            }
        }

        // POST api/Paperless/UpdateQuestion
        [HttpPost("UpdateQuestion")]
        public ActionResult UpdateQuestion(Question question)
        {
            var sql = @"
            UPDATE QUESTION SET
                PROMPT = @prompt
            WHERE
                ID = @id;
            SELECT * FROM QUESTION WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newQuestion= db.Query<Question>(sql, question).First();
                return Ok(newQuestion);
            }
        }

        // POST api/Paperless/DeleteQuestion
        [HttpPost("DeleteQuestion/{id}")]
        public ActionResult DeleteQuestion(int id)
        {
            var sql = @"DELETE FROM QUESTION WHERE ID = @id;";

            using (var db = new SqlConnection(ConnectionString))
            {
                db.Execute(sql, new {id = id});
                return Ok();
            }
        }

        #endregion

        #region Pack

        // GET api/Paperless/GetPacks
        [HttpGet("GetPacks")]
        public ActionResult<IEnumerable<Form>> GetPacks()
        {
            var sql = "SELECT * FROM PACK";

            using (var db = new SqlConnection(ConnectionString))
            {
                var packs = db.Query<Pack>(sql);
                return Ok(packs);
            }
        }

        // GET api/Paperless/CreatePack
        [HttpPost("CreatePack")]
        public ActionResult CreatePack(Pack pack)
        {
            var sql = @"
            DECLARE @NEXT INT;
            SELECT @NEXT = NEXT VALUE FOR SEQ_PACK;
            INSERT INTO PACK (
                ID, LINE_NO, MATERIAL_NO, PRODUCTION_DATE
            ) VALUES (
                @NEXT, @lineNo, @materialNo, @productionDate
            );
            SELECT * FROM PACK WHERE ID = @NEXT;";

            using (var db = new SqlConnection(ConnectionString))
            {
                var newPack = db.Query<Pack>(sql, pack).First();

                // Automatically add the forms required for the pack based on pre-configuration.

                // Get list of ALL forms
                var forms = _GetForms();

                // Track which forms already added to pack
                List<int> formsAdded = new List<int>();

                var assignments = db.Query<FormAssignment>("SELECT * FROM FORM_ASSIGNMENT");
                foreach (var assignment in assignments)
                {
                    Regex re_form = new Regex(assignment.MATERIAL_NO_REGEX);
                    Regex re_material = new Regex(assignment.MATERIAL_NO_REGEX);
                    Regex re_line = new Regex(assignment.LINE_NO_REGEX);

                    foreach (var form in forms)
                    {
                        if (re_form.IsMatch(form.Name))
                        {
                            if (re_material.IsMatch(pack.MaterialNo) && re_line.IsMatch(pack.LineNo))
                            {
                                if (!formsAdded.Contains(form.Id))
                                {
                                    formsAdded.Add(form.Id);
                                    db.Execute(
                                        "INSERT INTO PACK_FORM (PACK_ID, FORM_ID) VALUES (@PACK_ID, @FORM_ID)",
                                        new { PACK_ID = newPack.Id, FORM_ID = form.Id }
                                    );
                                }
                            }
                        }
                    }
                }

                // Automatically add NULL answers for all questions in all forms in pack.
                // Additional answers for questions in tabular sections can be added through
                // AddTableRow operation:
                sql = @"
                INSERT INTO ANSWER (ID, QUESTION_ID, PACK_ID, SEQ)
                SELECT
                    NEXT VALUE FOR SEQ_ANSWER,
                    Q.ID,
                    @packId,
                    1
                FROM
                    QUESTION Q
                INNER JOIN
                    SECTION S
                ON
                    Q.SECTION_ID = S.ID
                INNER JOIN
                    FORM F
                ON
                    S.FORM_ID = F.ID
                INNER JOIN
                    PACK_FORM PF
                ON
                    F.ID = PF.FORM_ID
                WHERE
                    PF.PACK_ID = @packId";

                db.Execute(sql, new { packId = newPack.Id });

                return Ok(newPack);
            }
        }

        #endregion

        #region Answers

        // GET api/Paperless/GetAnswerRagsForSection
        [HttpGet("GetAnswerRagsForSection/{packId}/{sectionId}")]
        public ActionResult<IEnumerable<AnswerRag>> GetAnswerRagsForSection(int packId, int sectionId)
        {
            var targets = _GetTargets();
            var thresholds = GetTargetThresholds();
            List<AnswerRag> results = new List<AnswerRag>();

            var sql = @"
            SELECT
                A.ID,
                P.LINE_NO,
                P.MATERIAL_NO,
                A.VALUE,
                Q.QUESTION_TYPE,
                Q.TARGET_NAME
            FROM
                ANSWER A
            INNER JOIN
                PACK P
            ON
                A.PACK_ID = P.ID
            INNER JOIN
                QUESTION Q
            ON
                A.QUESTION_ID = Q.ID
            WHERE
                PACK_ID = @packId AND
                QUESTION_ID IN (SELECT ID FROM QUESTION WHERE SECTION_ID = @sectionId)";

            using (var db = new SqlConnection(ConnectionString))
            {
                // Get all answers in section with the target name
                var answers = db.Query<dynamic>(sql, new
                {
                    packId = packId,
                    sectionId = sectionId
                });

                // For each answer, get the RAG status:
                foreach (var answer in answers)
                {
                    bool isNumeric = false;
                    float valueToCompare;
                    isNumeric = float.TryParse(answer.VALUE, out valueToCompare);

                    var target = targets.FirstOrDefault(t => t.TargetName == answer.TARGET_NAME);

                    // If answer question does not have target, or value entered is not numeric, set to WHITE
                    if (target == null || !isNumeric)
                    {
                        results.Add(new AnswerRag()
                        {
                            Id = answer.ID,
                            Rag = "WHITE"
                        });
                    }
                    else
                    {
                        // Get all threshold values
                        foreach (var thresholdRule in thresholds.Where(t => t.TargetName == answer.TARGET_NAME).OrderBy(t => t.EvaluationOrder))
                        {
                            // does the rule match this answer's pack?
                            Regex re_material = new Regex(thresholdRule.MaterialNoRegex);
                            Regex re_line = new Regex(thresholdRule.LineNoRegex);
                            if (re_material.IsMatch(answer.MATERIAL_NO) && re_line.IsMatch(answer.LINE_NO))
                            {
                                var color = "AMBER"; // default
                                // found the first rule - determine RAG color
                                if (
                                    (target.TargetType == "HIGHER" && valueToCompare >= thresholdRule.GreenThreshold1) ||
                                    (target.TargetType == "LOWER" && valueToCompare <= thresholdRule.GreenThreshold1) ||
                                    (target.TargetType == "IN RANGE" && valueToCompare >= thresholdRule.GreenThreshold1 && valueToCompare <= thresholdRule.GreenThreshold2) ||
                                    (target.TargetType == "NOT IN RANGE" && !(valueToCompare >= thresholdRule.GreenThreshold1) && !(valueToCompare <= thresholdRule.GreenThreshold2))
                                )
                                {
                                    color = "GREEN";
                                }
                                else if (
                                  (target.TargetType == "HIGHER" && valueToCompare <= thresholdRule.RedThreshold1) ||
                                  (target.TargetType == "LOWER" && valueToCompare >= thresholdRule.RedThreshold1) ||
                                  (target.TargetType == "IN RANGE" && !(valueToCompare >= thresholdRule.RedThreshold1) && !(valueToCompare <= thresholdRule.RedThreshold2)) ||
                                  (target.TargetType == "NOT IN RANGE" && valueToCompare >= thresholdRule.RedThreshold1 && valueToCompare <= thresholdRule.RedThreshold2)
                                )
                                {
                                    color = "RED";
                                }

                                results.Add(new AnswerRag()
                                {
                                    Id = answer.ID,
                                    Rag = color
                                });
                            }
                        }
                    }
                }
            }
            return Ok(results);
        }


        // GET api/Paperless/GetAnswersForSection
        [HttpGet("GetAnswersForSection/{packId}/{sectionId}")]
        public ActionResult<IEnumerable<Answer>> GetAnswersForSection(int packId, int sectionId)
        {
            var sql = "SELECT * FROM ANSWER WHERE PACK_ID = @packId AND QUESTION_ID IN (SELECT ID FROM QUESTION WHERE SECTION_ID = @sectionId)";

            using (var db = new SqlConnection(ConnectionString))
            {
                var answers = db.Query<Answer>(sql, new
                {
                    packId = packId,
                    sectionId = sectionId
                });
                return Ok(answers);
            }
        }


        // GET api/Paperless/GetAnswersForQuestion
        [HttpGet("GetAnswersForQuestion/{packId}/{questionId}")]
        public ActionResult<IEnumerable<Form>> GetAnswersForQuestion(int packId, int questionId)
        {
            var sql = "SELECT * FROM ANSWER WHERE PACK_ID = @packId AND QUESTION_ID = @questionId";

            using (var db = new SqlConnection(ConnectionString))
            {
                var answers = db.Query<Form>(sql, new
                {
                    packId = packId,
                    questionId = questionId
                });
                return Ok(answers);
            }
        }

        // GET api/Paperless/GetTableSize
        [HttpGet("GetTableSize/{packId}/{sectionId}")]
        public ActionResult GetTableSize(int packId, int sectionId)
        {
            var sql = @"
            SELECT MAX(SEQ) ROWS
            FROM
                ANSWER A
            INNER JOIN
                QUESTION Q
            ON
                A.QUESTION_ID = Q.ID
            INNER JOIN
                SECTION S
            ON
                Q.SECTION_ID = S.ID
            WHERE
                A.PACK_ID = @packId AND
                S.ID = @sectionId";

            using (var db = new SqlConnection(ConnectionString))
            {
                // Add / update answer, and return the entire section the question/answer was in
                var size = db.ExecuteScalar(sql, new
                {
                    packId = packId,
                    sectionId = sectionId
                });

                // return the entire section
                return Ok(new
                {
                    size = size
                });
            }
        }

        // GET api/Paperless/AnswerQuestion
        [HttpPost("AddTableRow/{packId}/{sectionId}")]
        public ActionResult AddTableRow(int packId, int sectionId)
        {
            var sql = @"
            SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
            BEGIN TRANSACTION

                DECLARE @NEXT_SEQ INT
                SELECT
                    @NEXT_SEQ = COALESCE(MAX(SEQ),0)+1
                FROM
                    ANSWER A
                INNER JOIN
                    QUESTION Q
                ON
                    A.QUESTION_ID = Q.ID
                INNER JOIN
                    SECTION S
                ON
                    Q.SECTION_ID = S.ID
                WHERE
                    A.PACK_ID = @packId AND
                    S.ID = @sectionId

                INSERT INTO
                    ANSWER (ID, QUESTION_ID, PACK_ID, SEQ)
                SELECT
                    NEXT VALUE FOR SEQ_ANSWER,
                    Q.ID,
                    @packId,
                    @NEXT_SEQ
                FROM
                    SECTION S
                INNER JOIN
                    QUESTION Q
                ON
                    Q.SECTION_ID = S.ID AND
                    S.ID = @sectionID

                -- RETURN ANSWERS FOR THE SECTION
                SELECT
                    *
                FROM
                    ANSWER
                WHERE
                    PACK_ID = @packId AND
                    QUESTION_ID IN (
                        SELECT ID FROM QUESTION WHERE SECTION_ID = @sectionId
                    )
            COMMIT";

            using (var db = new SqlConnection(ConnectionString))
            {
                // Add / update answer, and return the entire section the question/answer was in
                var answers = db.Query<Answer>(sql, new
                {
                    packId = packId,
                    sectionId = sectionId
                });
                return Ok(answers);
            }
        }

        // GET api/Paperless/AnswerQuestion
        [HttpPost("AnswerQuestion")]
        public ActionResult AnswerQuestion(Answer answer)
        {
            var sql = @"
                UPDATE ANSWER SET
                    ANSWER_BY_PROD = @answerByProd,
                    ANSWER_BY_TECH = @answerByTech,
                    ANSWER_DT = GETDATE(),
                    VALUE = @value,
                    COMMENT = @comment
                WHERE
                    ID = @id;

                -- Return new answer
                SELECT
                    *
                FROM
                    ANSWER
                WHERE
                    ID = @id";

            using (var db = new SqlConnection(ConnectionString))
            {
                // Add / update answer, and return the answer
                return Ok(db.Query<Answer>(sql, answer).First());
            }
        }

        #endregion

        #region Others

        // GET api/Paperless/GetList
        [HttpGet("GetList")]
        public ActionResult GetListItems()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var list = db.Query<ListItem>(
                    "SELECT * FROM LIST"
                );
                return Ok(list);
            }
        }

        // GET api/Paperless/GetList
        [HttpGet("GetTargets")]
        public ActionResult GetTargets()
        {
            return Ok(_GetTargets());
        }

        private IEnumerable<Target> _GetTargets()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var targets = db.Query<Target>(
                    "SELECT * FROM TARGET"
                );
                return targets;
            }
        }

        private IEnumerable<TargetThreshold> GetTargetThresholds()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var targets = db.Query<TargetThreshold>(
                    "SELECT * FROM TARGET_THRESHOLD"
                );
                return targets;
            }
        }

        #endregion
    }
}