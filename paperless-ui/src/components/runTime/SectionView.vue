<template>
    <div class='section'>

      <div v-if='section !== undefined && answers !== undefined && tableSize !== undefined && answerRags !== undefined'>

      <hr />
      <div v-text='section.name' style='float:right; font-style:italic; font-weight: 600; font-size: small;' />

      <!-- Tabular questions -->
      <div class='section' v-if='section.tabularFlag===true'>

        <table>
          <tr>
            <th v-for='question in questions' v-bind:key='question.id'>{{ question.prompt }}</th>
          </tr>

          <tr v-for='row in tableSize' v-bind:key='row'>

            <td v-for='question in questions' v-bind:key='question.id'>

              <div v-if="question.questionType==='D'">
              <input type="date" v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
              </div>

              <div v-if="question.questionType==='T'">
              <input type="time" v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
              </div>

              <div v-if="question.questionType==='S'">
              <input type="text" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, row+0).id) ]' v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
              </div>

              <div v-if="question.questionType==='M'">
              <textarea rows="5" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, row+0).id) ]' v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))" />
              </div>

              <div v-if="question.questionType==='N'">
              <input type="text" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, row+0).id) ]' v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
              </div>

              <div v-if="question.questionType==='C'">
              <input type="checkbox" v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
              </div>

              <div v-if="question.questionType==='P'">
                <select v-model='getAnswerForQuestion(question.id, row+0).value' @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
                  <option v-for='option in getList(question.listName)' v-bind:key='option.listValue' v-bind:value='option.listValue'>
                    {{ option.listText }}
                  </option>
                </select>
              </div>

              <div v-if="question.questionType==='B'">
                <select @blur="saveAnswer(getAnswerForQuestion(question.id, row+0))">
                  <option v-for='option in getList("YES_NO_NA")' v-bind:key='option.listValue' v-bind:value='option.listValue'>
                    {{ option.listText}}
                  </option>
                </select>
              </div>

            </td>

          </tr>
        </table>

        <button class='button' v-on:click='addRow(section.id, packId)'>Add Row</button>

      </div>

      <!-- Normal questions -->
      <div v-if='section.tabularFlag===false'>
        <div class='formQuestion' v-for='question in questions' v-bind:key='question.id' >
          <label :for="question.questionId" v-text='question.prompt' />

          <!-- Can only have 1 answer per question -->
              <div v-if="question.questionType==='D'">
              <input type="date" :questionId="question.id" :value='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
              </div>

              <div v-if="question.questionType==='T'">
              <input type="time" :questionId="question.id" :value='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
              </div>

              <div v-if="question.questionType==='S'">
              <input type="text" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, 1).id) ]'  v-model='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
              </div>

              <div v-if="question.questionType==='M'">
              <textarea rows="5" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, 1).id) ]' v-model='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))" />
              </div>

              <div v-if="question.questionType==='N'">
              <input type="text" v-bind:class='[ getRagClassForAnswer(getAnswerForQuestion(question.id, 1).id) ]' v-model='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
              </div>

              <div v-if="question.questionType==='C'">
              <input type="checkbox" :questionId="question.id" :value='getAnswerForQuestion(question.id, 1).value' v-model='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
              </div>

              <div v-if="question.questionType==='P'">
                <select :questionId="question.id" packId="6" v-model='getAnswerForQuestion(question.id, 1).value' @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
                  <option v-for='option in getList(question.listName)' v-bind:key='option.listValue' v-bind:value='option.listValue'>
                    {{ option.listText}}
                  </option>
                </select>
              </div>

              <div v-if="question.questionType==='B'">
                <select :questionId="question.id" packId="6" @blur="saveAnswer(getAnswerForQuestion(question.id, 1))">
                  <option v-for='option in getList("YES_NO_NA")' v-bind:key='option.listValue' v-bind:value='option.listValue'>
                    {{ option.listText}}
                  </option>
                </select>
              </div>
        </div>
      </div>
</div>
    </div>
</template>

<script>
var axios = require('axios');

export default {
  name: 'SectionView',
  props: {
    section: {
      type: Object
    },
    packId: {
      type: Number
    },
    list: {
      type: Array,
      default: function() {
        return [];
      }
    }
  },
  data: function() {
    return {
      questions: undefined,
      tableSize: undefined,
      answers: undefined,
      answerRags: undefined
    };
  },
  filters: {
    completedAnswerTitle: function(answerDt) {
      return 'Answered on ' + answerDt;
    }
  },
  watch: {
    section: function(val) {
      this.getQuestions();
    }
  },
  created: function() {
    this.getData();
  },
  methods: {
    getTableSize: function() {
      return axios.get(
        process.env.VUE_APP_API_ROOT +
          '/api/paperless/getTableSize/' +
          this.packId +
          '/' +
          this.section.id
      );
    },
    getAnswers: function() {
      return axios.get(
        process.env.VUE_APP_API_ROOT +
          '/api/paperless/getAnswersForSection/' +
          this.packId +
          '/' +
          this.section.id
      );
    },
    getAnswerRags: function() {
      return axios.get(
        process.env.VUE_APP_API_ROOT +
          '/api/paperless/getAnswerRagsForSection/' +
          this.packId +
          '/' +
          this.section.id
      );
    },
    getQuestions: function() {
      return axios.get(
        process.env.VUE_APP_API_ROOT +
          '/api/paperless/getQuestions/' +
          this.section.id
      );
    },
    getData: function() {
      if (this.section !== undefined) {
        let that = this;
        this.getQuestions()
          .then(function(response) {
            that.questions = response.data;
            return that.getTableSize();
          })
          .then(function(response) {
            that.tableSize = response.data.size;
            return that.getAnswers();
          })
          .then(function(response) {
            that.answers = response.data;
            return that.getAnswerRags();
          })
          .then(function(response) {
            that.answerRags = response.data;
          })
          .catch(function(error) {
            alert('whoops');
            console.log(error.message);
          });
      }
    },
    getRagClassForAnswer: function(answerId) {
      var rag = this.answerRags.filter(function(a) { return a.id === answerId })[0].rag;
      return 'rag' + rag;
    },
    // Gets the answers for a specific question and row sequence
    getAnswerForQuestion: function(questionId, seq) {
      var answer = this.answers.filter(function(a) {
        return a.questionId === questionId && a.seq === seq;
      })[0];
      return answer;
    },
    getList: function(listName) {
      return this.list.filter(function(item) {
        return item.listName === listName;
      });
    },
    addRow: function(sectionId, packId) {
      let that = this;
      axios
        .post(process.env.VUE_APP_API_ROOT + '/api/paperless/addTableRow/' + packId + '/' + sectionId)
        .then(function(response) {
          that.answers = undefined; // hides the panel whilst update takes place
          that.answerRags = undefined; // hides the panel whilst update takes place
          that.tableSize = undefined;
          that.getData();
        });
    },
    saveAnswer: function(answer) {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/answerQuestion',
          answer
        )
        .then(function(response) {
          // replace existing section
          that.getData();
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
div.section {
  padding: 4px;
}

div.question {
  margin-bottom: 14px;
}

.ragRED {
  background: rgb(255, 180, 180);
}

.ragAMBER {
  background: rgb(255, 255, 180);
}

.ragGREEN {
  background: rgb(204, 255, 180);
}

.ragWHITE {
  background: rgb(255, 255, 255);
}

td select, td input {
  width: 100%;
  min-height: 30px;
  height: 30px;
  margin: 1px;
  min-width: 80px;
  box-sizing: border-box;
  padding-left: 2px;
}

</style>
