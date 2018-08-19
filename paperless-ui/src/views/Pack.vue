<template>
  <div class="pack">
    <PackView />
  </div>
</template>

<script>
// @ is an alias to /src
import PackView from '@/components/runTime/PackView.vue'

export default {
  name: 'pack',
  components: {
    PackView
  }
}
</script>
