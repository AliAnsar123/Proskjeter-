import Vue from 'vue'
import VueRouter from 'vue-router'
import Order from '../views/Order.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'order',
    component: Order
  },
  {
    path: '/admin',
    name: 'admin',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/Admin.vue')
  },
  {
    path: '/login',
    name: 'login',
    component: () => import('../views/Login.vue')
  },
  {
    path: '/receipt',
    name: 'receipt',
    component: () => import('../views/Receipt.vue'),
    props: (route) => ({
      ...route.params
    })
  },
  {
    path: '/admin/orders/:id',
    name: 'orderview',
    component: () => import('../views/OrderView.vue')
  },
  {
    path: '/admin/companies/new',
    name: 'newcompany',
    component: () => import('../views/CreateCompany.vue')
  },
  {
    path: '/admin/companies/:id',
    name: 'editcompany',
    component: () => import('../views/EditCompany.vue')
  },
  {
    path: '/admin/customers/new',
    name: 'createcustomer',
    component: () => import('../views/CreateCustomer.vue')
  },
  {
    path: '/admin/customers/:id',
    name: 'editcustomer',
    component: () => import('../views/EditCustomer.vue')
  },
  {
    path: '/admin/ports/new',
    name: 'createport',
    component: () => import('../views/CreatePort.vue')
  },
  {
    path: '/admin/ports/:id',
    name: 'editport',
    component: () => import('../views/EditPort.vue')
  },
  {
    path: '/admin/routes/new',
    name: 'createroute',
    component: () => import('../views/CreateRoute.vue')
  },
  {
    path: '/admin/routes/:id',
    name: 'editroute',
    component: () => import('../views/EditRoute.vue')
  },
  {
    path: '/admin/routetimes/new',
    name: 'createroutetime',
    component: () => import('../views/CreateRouteTime.vue')
  },
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
