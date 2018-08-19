<template>
  <div>
      <h2>Pack</h2>

      <!-- Select Pack -->
      <div class='packSelector;'>
        <label>Select Pack:</label>
        <select v-model='selectedPack'>
          <option v-for='pack in packs' v-bind:key='pack.id' :value='pack' v-text='fullPackName(pack)' />
       </select>
      </div>

      <div class='formBody'>

        <div class='formSelector'>
          <!-- Links to each form in the pack -->
          <div v-bind:key='form.id' v-for='form in forms'>
              <div class='formSelectButton' v-bind:href='form.id' v-on:click='selectForm(form.id)'>{{ form.description }}</div>
          </div>
        </div>

        <div class='formSeparator'>&nbsp;</div>

        <div class='formEdit' v-if='this.selectedForm !== null'>
          <!-- Current form being viewed -->
          <FormView :form='selectedForm' :packId='selectedPack.id' />
        </div>
      </div>
  </div>
</template>

<script>
import FormView from '@/components/runTime/FormView.vue';
var axios = require('axios');

export default {
  name: 'PackView',
  components: {
    FormView
  },
  data: function() {
    return {
      packs: [],
      selectedPack: {},
      forms: [],
      selectedForm: null
    };
  },
  props: {
    packId: Number
  },
  watch: {
    // When user changes the selectedPack, we watch this
    // and set the pack data item to be the FULL pack
    // graph.
    selectedPack: function(newVal, oldVal) {
      let that = this;
      axios
        .get(
          process.env.VUE_APP_API_ROOT +
            '/api/paperless/getPackForms/' +
            newVal.id
        )
        .then(function(response) {
          that.forms = response.data;
          if (that.forms.length > 0) {
            that.selectForm(that.forms[0].id);
          }
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  },
  methods: {
    fullPackName: function(pack) {
      return (
        'Production Date:' +
        new Date(pack.productionDate).toLocaleDateString() +
        '; Line: ' +
        pack.lineNo +
        '; Material No:' +
        pack.materialNo
      );
    },
    selectForm: function(formId) {
      this.selectedForm = this.forms.filter(f => f.id === formId)[0];
    }
  },
  created() {
    var that = this;
    axios
      .get(process.env.VUE_APP_API_ROOT + '/api/paperless/getpacks')
      .then(function(response) {
        that.packs = response.data;
      })
      .catch(function(error) {
        console.log(error.message);
      });
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
div.packSelector {
  width: 400px;
}

div.formBody {
  width: 100%;
  max-width: 100%;
  margin: 0 auto;
  box-sizing: border-box;
  margin-top: 5px;
  min-height: 300px;
}

div.formSelector {
  width: 15%;
  box-sizing: border-box;
  display: inline-block;
}

div.formSeparator {
  width: 1%;
  box-sizing: border-box;
  display: inline-block;
}

div.formEdit {
  width: 84%;
  box-sizing: border-box;
  display: inline-block;
  vertical-align: top;
}

div.formSelectButton {
  display: flex;
  justify-content: center;
  align-content: center;
  flex-direction: column;
  min-height: 30px;
  padding: 2px 4px;
  text-align: center;
  font-size: 12px;
  font-weight: 700;
  cursor: pointer;
  box-sizing: border-box;
  border: 1px solid;
  margin-bottom: 4px;
}

div.formSelectButton:hover {
  color: #222;
  background-color: rgb(153, 204, 255); /* #33C3F0; */
}

ul.tab-nav {
  list-style: none;
  padding-left: 5px;
}

ul.tab-nav li {
  display: inline;
}

ul.tab-nav a {
  text-decoration: none;
}
</style>
