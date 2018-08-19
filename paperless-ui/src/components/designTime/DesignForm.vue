<template>
    <div>
        <legend>Design Forms</legend>

        <fieldset>

          <!-- View Form -->
          <label>Forms:</label>
          <div v-if='mode==="VIEW"'>
            <select v-model='selectedForm' >
              <option v-for='form in forms' v-bind:key='form.name' :value='form' v-text='form.description' />
            </select>
            <button v-on:click='addNew'>Add Form</button>
            <label>Name:</label>
            <div v-text='selectedForm.name'/>
            <label>Description:</label>
            <div v-text='selectedForm.description'/>
            <div class='buttonGroup'>
                <button v-on:click='doEdit'>Edit</button>
                <button v-on:click='doDelete'>Delete</button>
            </div>
            <DesignSection v-bind:form='selectedForm' />
          </div>

          <!-- Edit Form -->
          <div v-if='mode==="EDIT"'>
              <label>Name:</label>
              <input style="width:50%;" v-model='editForm.name' />
              <label>Description:</label>
              <input style="width:50%;" v-model='editForm.description' />
              <div class='buttonGroup'>
                  <button v-on:click='doSaveEdit'>Save</button>
                  <button v-on:click='doCancelEdit'>Cancel</button>
              </div>
              <DesignSection v-bind:form='selectedForm' />
          </div>

          <!-- New Form -->
          <form class='new' v-if='mode==="NEW"'>
              <fieldset>
                  <label>Name:</label>
                  <input type='text' name='name' v-model='newForm.name' />
                  <label>Description:</label>
                  <input type='text' name='description' v-model='newForm.description' />
                  <div class='buttonGroup'>
                      <button @click='doSaveNew'>Save</button>
                      <button @click='doCancelNew'>Cancel</button>
                  </div>
              </fieldset>
          </form>

        </fieldset>
    </div>
</template>

<script>
import DesignSection from '@/components/designTime/DesignSection.vue';
var axios = require('axios');

export default {
  name: 'DesignForm',
  components: {
    DesignSection
  },
  data: function() {
    return {
      forms: {},
      mode: 'VIEW',
      editForm: undefined,
      selectedForm: {
        type: Object
      },
      newForm: {}
    };
  },
  methods: {
    getForms: function() {
      let that = this;
      axios
        .get(process.env.VUE_APP_API_ROOT + '/api/paperless/getForms')
        .then(function(response) {
          that.forms = response.data;
          if (that.forms.length > 0) {
            that.mode = 'VIEW';
            that.selectedForm = that.forms[0];
          }
        })
        .catch(function(error) {
          console.log(error.message);
        });
    },
    doEdit: function() {
      this.editForm = Object.assign({}, this.selectedForm);
      this.mode = 'EDIT';
    },
    doDelete: function() {
      let that = this;
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/deleteForm/' + this.selectedForm.id
        )
        .then(function(response) {
          that.editForm = undefined;
          that.getForms();
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
          process.env.VUE_APP_API_ROOT + '/api/paperless/updateForm',
          this.editForm
        )
        .then(function(response) {
          that.mode = 'VIEW';
          that.editForm = undefined;
          that.getForms();
        })
        .catch(function(error) {
          console.log(error.message);
        });
      this.mode = 'VIEW';
    },
    addNew: function() {
      this.mode = 'NEW';
    },
    doSaveNew: function(event) {
      event.preventDefault();
      let that = this;
      var payload = {
        name: this.newForm.name,
        description: this.newForm.description,
        authorisedBy: 'David Barone',
        createdBy: 'David Barone',
        createdDt: '2018-07-23T16:16:01.87',
        updatedBy: 'David Barone',
        updatedDt: '2018-07-23T16:16:01.87',
        version: 1,
        effectiveDt: '2018-12-15T00:00:00',
        expiryDt: '9999-12-31T00:00:00'
      };
      axios
        .post(
          process.env.VUE_APP_API_ROOT + '/api/paperless/createForm',
          payload
        )
        .then(function(response) {
          var newForm = response.data;
          that.forms.push(newForm);
          that.mode = 'VIEW';
          that.selectedForm = newForm;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    },
    selectForm: function(form) {
      this.selectedForm = form;
    }
  },
  mounted() {
    this.getForms();
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
label {
  font-weight: 600;
}
</style>
