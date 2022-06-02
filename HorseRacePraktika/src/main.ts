import 'jquery';
import 'bootstrap';
// import 'simplebar';
// import 'simplebar/dist/simplebar.css';
// import VueSimplebar from 'vue-simplebar'

import 'vue-simplebar/dist/vue-simplebar.min.css'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'font-awesome/css/font-awesome.min.css'
import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";

const app = createApp(App);

app.use(router);

app.mount("#app");
