<template>
    <div>
        <hr />
        <label>Sections:</label>

        <!-- View Section -->
        <div v-if='mode==="VIEW"'>
          <select v-model='selectedSection'>
              <option v-for='section in sections' v-bind:key='section.id' :value='section' v-text='section.name' />
          </select>
          <button v-on:click='addNew'>Add Section</button>

          <label>Name:</label>
          <div v-text='selectedSection.name'/>
          <label>Tabular Mode:</label>
          <input type='checkbox' disabled :v-model='selectedSection.tabularMode'/>

          <div class='buttonGroup'>
              <button v-on:click='doEdit'>Edit</button>
              <button v-on:click='doDelete'>Delete</button>
          </div>
          <DesignQuestion v-bind:section='selectedSection' />
        </div>

        <!-- Edit Section -->
        <div v-if='mode==="EDIT"'>
            <label>Name:</label>
            <input v-model='editSection.name' />
            <label>Tabular Mode:</label>
            <input type='checkbox' :v-model='selectedSection.tabularMode'/>
            <div class='buttonGroup'>
              <button v-on:click='doSaveEdit'>Save</button>
              <button v-on:click='doCancelEdit'>Cancel</button>
            </div>
            <DesignQuestion v-bind:section='selectedSection' />
        </div>

        <!-- New Section -->
        <form v-if='mode==="NEW"'>
          <fieldset>
            <label>Name:</label>
            <input type='text' name='name' v-model='newSection.name' />
            <label>Tabular Mode:</label>
            <input type='checkbox' name='description' v-model='newSection.tabularFlag' />
            <div class='buttonGroup'>
              <button @click='doSaveNew'>Save</button>
            </div>
          </fieldset>
        </form>
    </div>
</template>

<script>
import DesignQuestion from '@/components/designTime/DesignQuestion.vue';
var axios = require('axios');

export default {
  name: 'DesignSection',
  components: {
    DesignQuestion
  },
  props: ['form'],
  data: function() {
    return {
      sections: [],
      selectedSection: {},
      newSection: {},
      editSection: undefined,
      mode: 'VIEW'
    };
  },
  watch: {
    // whenever form changes (called by parent), run this function
    // to refresh sections to make 'reactive'. This is how you need
    // to make the prop reactive.
    form: function(newVal, oldVal) {
      this.refreshSections(this.form.id);
    }
  },
  created: function() {
    this.refreshSections(this.form.id);
  },
  methods: {
    doEdit: function() {
      this.editSection = Object.assign({}, this.selectedSection);
      this.mode = 'EDIT';
    },
    doDelete: function() {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/deleteSection/' + this.selectedSection.id
        )
        .then(function(response) {
          that.editSection = undefined;
          that.refreshSections(that.form.id);
          that.mode = 'VIEW';
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
    doSaveEdit: function() {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/updateSection',
          this.editSection
        )
        .then(function(response) {
          that.mode = 'VIEW';
          that.selectedSection = response.data;
          that.editSection = undefined;
          that.refreshSections(that.form.id);
        })
        .catch(function(error) {
          console.log(error.message);
        });
      this.mode = 'VIEW';
    },
    refreshSections: function(formId) {
      if (formId !== undefined) {
        let that = this;
        axios
          .get(
            process.env.VUE_APP_API_ROOT +
              '/api/paperless/getSections/' +
              formId
          )
          .then(function(response) {
            that.sections = response.data;
            if (that.sections.length > 0) {
              that.view = 'VIEW';
              that.selectedSection = that.sections[0];
            }
          })
          .catch(function(error) {
            console.log(error.message);
          });
      }
    },
    addNew: function() {
      this.newSection = {};
      this.mode = 'NEW';
    },
    doSaveNew: function(event) {
      let that = this;
      var payload = {
        name: this.newSection.name,
        tabularFlag: this.newSection.tabularFlag,
        formId: this.form.id
      };
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/createSection',
          payload
        )
        .then(function(response) {
          that.sections = response.data;
          // from the response, get the new section, with its newly created id.
          that.newSection = that.sections.filter(function(item) {
            return item.name === payload.name;
          })[0];
          that.mode = 'VIEW';
          that.selectedSection = that.newSection;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  },
  selectSection: function(section) {
    this.selectedSection = section;
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
