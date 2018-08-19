<template>
    <div>
        <hr />
        <label>Questions:</label>

        <div v-if='mode==="VIEW"'>
          <select v-model='selectedQuestion'>
              <option v-for='question in questions' v-bind:key='question.id' :value='question' v-text='question.prompt' />
          </select>

          <button v-on:click='doAddNew'>Add Question</button>

          <label>Question Type:</label>
          <select disabled v-model='selectedQuestion.questionType'>
            <option value="S">Text</option>
            <option value="M">Multiline Text</option>
            <option value="C">Completion (checkbox)</option>
            <option value="N">Numeric</option>
            <option value="T">Time</option>
            <option value="D">Date</option>
            <option value="P">Picklist</option>
          </select>
          <label>List Name:</label>
          <div v-text='selectedQuestion.listName'/>
          <label>Prompt:</label>
          <div v-text='selectedQuestion.prompt'/>
          <label>Target Name:</label>
          <select disabled v-model='selectedQuestion.targetName'>
            <option v-for='target in targets' :key='target.targetName' v-bind:value='target.targetName'>{{ target.targetName }}</option>
          </select>

          <div class='buttonGroup'>
              <button v-on:click='doEdit'>Edit</button>
              <button v-on:click='doDelete'>Delete</button>
          </div>

        </div>

        <!-- Edit Question -->
        <div class='edit' v-if='mode==="EDIT"'>
            <label>Question Type:</label>
            <select v-model='editQuestion.questionType'>
              <option value="S">Text</option>
              <option value="M">Multiline Text</option>
              <option value="C">Completion (checkbox)</option>
              <option value="N">Numeric</option>
              <option value="T">Time</option>
              <option value="D">Date</option>
              <option value="P">Picklist</option>
            </select>
            <label v-if='editQuestion.questionType==="P"'>List Name:</label>
            <input type='text' v-if='editQuestion.questionType==="P"' name='listName' v-model='editQuestion.listName' />
            <label>Prompt:</label>
            <input type='text' name='prompt' v-model='editQuestion.prompt' />
            <label>Target Name:</label>
            <select v-model='editQuestion.targetName'>
              <option v-for='target in targets' :key='target.targetName' v-bind:value='target.targetName'>{{ target.targetName }}</option>
            </select>
            <div class='buttonGroup'>
              <button v-on:click='doSaveEdit'>Save</button>
              <button v-on:click='doCancelEdit'>Cancel</button>
            </div>
        </div>

        <!-- New Question -->
        <form class='new' v-if='mode==="NEW"'>
          <fieldset>
            <label>Question Type:</label>
            <select v-model='newQuestion.questionType'>
              <option value="S">Text</option>
              <option value="M">Multiline Text</option>
              <option value="C">Completion (checkbox)</option>
              <option value="N">Numeric</option>
              <option value="T">Time</option>
              <option value="D">Date</option>
              <option value="P">Picklist</option>
            </select>
            <label v-if='newQuestion.questionType==="P"'>List Name:</label>
            <input type='text' v-if='newQuestion.questionType==="P"' name='listName' v-model='newQuestion.listName' />
            <label>Prompt:</label>
            <input type='text' name='prompt' v-model='newQuestion.prompt' />
            <label>Target Name:</label>
            <select v-model='newQuestion.targetName'>
              <option v-for='target in targets' :key='target.targetName' v-bind:value='target.targetName'>{{ target.targetName }}</option>
            </select>
            <div class='buttonGroup'>
              <button @click='doSaveNew'>Save</button>
              <button @click='doCancelNew'>Cancel</button>
            </div>
          </fieldset>
        </form>
    </div>
</template>

<script>
var axios = require('axios');

export default {
  name: 'DesignQuestion',
  props: ['section'],
  data: function() {
    return {
      mode: 'VIEW',
      questions: [],
      selectedQuestion: {},
      newQuestion: {},
      isNewMode: {},
      targets: []
    };
  },
  watch: {
    // whenever section changes (called by parent), run this function
    // to refresh questions to make 'reactive'. This is how you need
    // to make the prop reactive.
    section: function(newVal, oldVal) {
      this.refreshQuestions(this.section.id)
    }
  },
  created: function() {
    this.refreshQuestions(this.section.id);
    this.getTargets();
  },
  methods: {
    doEdit: function() {
      this.editQuestion = Object.assign({}, this.selectedQuestion);
      this.mode = 'EDIT';
    },
    doDelete: function() {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/deleteQuestion/' + this.selectedSection.id
        )
        .then(function(response) {
          that.editQueestion = undefined;
          that.refreshQuestions(that.section.id);
          that.mode = 'VIEW';
        })
        .catch(function(error) {
          console.log(error.message);
        });
      this.mode = 'VIEW';
    },
    doSaveEdit: function() {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/updateQuestion',
          this.editQuestion
        )
        .then(function(response) {
          that.mode = 'VIEW';
          that.selectedSection = response.data;
          that.editSection = undefined;
          that.refreshQuestions(that.section.id);
        })
        .catch(function(error) {
          console.log(error.message);
        });
      this.mode = 'VIEW';
    },
    doCancelEdit: function() {
      this.mode = 'VIEW';
      event.preventDefault();
    },
    doCancelNew: function() {
      this.mode = 'VIEW';
      event.preventDefault();
    },
    getTargets: function() {
      let that = this;
      axios
        .get(
          process.env.VUE_APP_API_ROOT + '/api/paperless/getTargets/'
        )
        .then(function(response) {
          that.targets = response.data;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    },
    refreshQuestions: function(sectionId) {
      let that = this;
      axios
        .get(
          process.env.VUE_APP_API_ROOT + '/api/paperless/getQuestions/' + sectionId
        )
        .then(function(response) {
          that.questions = response.data;
          if (that.questions.length > 0) {
            that.isNewMode = false;
            that.selectedQuestion = that.questions[0];
          }
        })
        .catch(function(error) {
          console.log(error.message);
        });
    },
    doAddNew: function() {
      this.mode = 'NEW';
      this.newQuestion = {};
    },
    doSaveNew: function(event) {
      event.preventDefault();
      let that = this;
      var payload = {
        questionType: this.newQuestion.questionType,
        listName: this.newQuestion.listName,
        prompt: this.newQuestion.prompt,
        sectionId: this.section.id,
        targetName: this.newQuestion.targetName,
        effectiveDt: '01-Jan-2018',
        expiryDt: '31-Dec-2099'
      };
      axios
        .post(process.env.VUE_APP_API_ROOT + '/api/paperless/createQuestion', payload)
        .then(function(response) {
          console.log(response.data);
          var newQuestion = response.data;
          that.questions.push(newQuestion);
          that.mode = 'VIEW';
          that.selectedQuestion = newQuestion;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  },
  selectQuestion: function(question) {
    this.selectedQuestion = question;
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
label {
  font-weight: 600;
}
</style>
