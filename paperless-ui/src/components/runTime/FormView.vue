<template>
  <div class='singleForm'>
      <legend v-text="form.description" />
      <!-- Display each section in form -->
      <div v-for='section in sections' v-bind:key='section.name' >
        <SectionView :section=section :packId='packId' :list='Object.values(list)' />
      </div>
    <div>&nbsp;
      <div class='footer'>Form: {{ form.name }}</div>
    </div>
  </div>
</template>

<script>
import SectionView from '@/components/runTime/SectionView.vue'
var axios = require('axios');

export default {
  name: 'FormView',
  components: {
    SectionView
  },
  props: {
    form: {
      type: Object
    },
    packId: {
      type: Number
    }
  },
  data: function() {
    return {
      list: [],
      sections: []
    }
  },
  watch: {
    form: function (f) {
      this.getSections();
    }
  },
  methods: {
    getSections: function() {
      let that = this;
      axios
        .get(process.env.VUE_APP_API_ROOT + '/api/paperless/getSections/' + this.form.id)
        .then(function(response) {
          that.sections = response.data;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  },
  created: function() {
    let that = this;
    // Get all lookup lists
    axios
      .get(process.env.VUE_APP_API_ROOT + '/api/paperless/getList')
      .then(function(response) {
        that.list = response.data;
      })
      .catch(function(error) {
        console.log(error.message);
      });

    this.getSections();
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

div.footer {
  position: absolute;
  bottom: 4px;
  right: 4px;
  margin: 0px auto;
  font-size: small;
  text-align: center;
  font-weight: 600;
}

div.singleForm {
  border: 1px solid #aaa;
  background: #ccc;
  padding: 4px;
  border-radius: 4px;
  min-height: 400px;
  position:relative;
  margin-top: 6px;
}

</style>
