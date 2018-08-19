<template>
    <div>
        <legend>Design Targets</legend>

        <fieldset>

          <!-- View Form -->
          <label>Targets:</label>

            <select v-model='selectedTarget' >
                <option v-for='target in targets' v-bind:key='target.targetName' :value='target' v-text='target.targetName' />
            </select>

        </fieldset>
    </div>
</template>

<script>
import DesignThreshold from '@/components/designTime/DesignThreshold.vue';
var axios = require('axios');

export default {
  name: 'DesignTarget',
  components: {
    DesignThreshold
  },
  data: function() {
    return {
      targets: undefined,
      selectedTarget: undefined
    };
  },
  methods: {
    getTargets: function() {
      let that = this;
      axios
        .get(process.env.VUE_APP_API_ROOT + '/api/paperless/getTargets')
        .then(function(response) {
          that.targets = response.data;
        })
        .catch(function(error) {
          console.log(error.message);
        });
    }
  },
  created() {
    this.getTargets();
  }
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
label {
  font-weight: 600;
}
</style>
