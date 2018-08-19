<template>
    <div>
        <legend>Create Pack</legend>

        <!-- New Pack -->
        <form class='new' v-on:submit='saveNew'>
          <fieldset>
            <label>Material No:</label>
            <input type='text' name='materialNo' v-model='pack.materialNo' />
            <label>Line No:</label>
            <input type='text' name='lineNo' v-model='pack.lineNo' />
            <label>Production Date:</label>
            <input type='date' name='productionDate' v-model='pack.productionDate' />
            <div class='buttonGroup'>
              <button @click='saveNew'>Create</button>
            </div>
          </fieldset>
        </form>
    </div>
</template>

<script>
var axios = require('axios');

export default {
  name: 'CreatePack',
  data: function() {
    return {
      pack: {}
    };
  },
  methods: {
    saveNew: function(event) {
      event.preventDefault();
      let that = this;
      var payload = {
        materialNo: this.pack.materialNo,
        lineNo: this.pack.lineNo,
        productionDate: this.pack.productionDate
      };
      axios
        .post(process.env.VUE_APP_API_ROOT + '/api/paperless/createPack', payload)
        .then(function(response) {
          that.pack = {};
          alert('Pack created successfully!')
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
label {
  font-weight: 600;
}
</style>
