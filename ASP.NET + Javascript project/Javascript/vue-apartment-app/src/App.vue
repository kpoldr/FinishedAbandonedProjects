<template>
<body>
  <div class="dflex vh-100 container">
    <Header />

    <main role="main" class="pb-3">
      <RouterView />
    </main>

    
  </div>
  <Footer />  
</body>  
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";

import Header from "./components/Header.vue";
import Footer from "./components/Footer.vue";


@Options({
  components: {
    Header,
    Footer,
  },
})
export default class App extends Vue {
  
  

  // async mounted(): Promise<void> {

  //   this.personsStore.$state.persons = await this.personsService.getAll()

  //   this.personsService.getAll().then(
  //     data => {
  //     this.personsStore.$state.persons = data;
  //     }
  //   )
  // }
}
</script>

<style scoped>

body {
    
    min-height: 100vh;
    display: flex;
    flex-direction: column;
}

footer{
    margin-top: auto;
}
</style>