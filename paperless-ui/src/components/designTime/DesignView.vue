<template>
  <div class='body'>
    <h2>Design</h2>
    <div class='menu'>
      <div @click='menu="Design Forms"' class='button'>Design Forms</div>
      <div @click='menu="Create Pack"' class='button'>Create Pack</div>
      <div @click='menu="Manage Targets"' class='button'>Manage Targets</div>
    </div>
    <div class='separator' />
    <div class='design'>
      <DesignForm v-if='menu==="Design Forms"' />
      <CreatePack v-if='menu==="Create Pack"' />
      <DesignTarget v-if='menu==="Manage Targets"' />
    </div>
  </div>
</template>

<script>
import DesignForm from '@/components/designTime/DesignForm.vue';
import CreatePack from '@/components/designTime/CreatePack.vue';
import DesignTarget from '@/components/designTime/DesignTarget.vue';

export default {
  name: 'DesignView',
  components: {
    DesignForm,
    CreatePack,
    DesignTarget
  },
  data: function() {
    return {
      menu: 'Design Forms'
    };
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

div.design {
  border: 1px solid #aaa;
  background: #ccc;
  padding: 4px;
  border-radius: 4px;
  min-height: 400px;
  position:relative;
}

div.body {
  width: 100%;
  max-width: 100%;
  margin: 0 auto;
  box-sizing: border-box;
  min-height: 300px;
}

div.menu {
  width: 15%;
  box-sizing: border-box;
  display: inline-block;
}

div.separator {
  width: 1%;
  box-sizing: border-box;
  display: inline-block;
}

div.design {
  width: 84%;
  box-sizing: border-box;
  display: inline-block;
  vertical-align: top;
}

div.button {
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

div.button:hover {
  color: #222;
  background-color: rgb(153, 204, 255); /* #33C3F0; */
}

</style>
