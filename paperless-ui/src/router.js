import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Design from './views/Design.vue'
import Pack from './views/Pack.vue'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home
    },
    {
      path: '/design',
      name: 'design',
      component: Design
    },
    {
      path: '/pack',
      name: 'pack',
      component: Pack
    }
  ]
})
