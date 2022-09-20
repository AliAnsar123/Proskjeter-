new Vue({
    el: '#app',
    vuetify: new Vuetify(),
    components: {
        App : httpVueLoader('js/App.vue')
    }
})