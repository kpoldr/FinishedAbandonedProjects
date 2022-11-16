<template>
  <header>
    <nav b-7882z672yd class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
      <div b-7882z672yd class="container-fluid">
        <a class="navbar-brand" href="/">WebApp</a>
        <button b-7882z672yd class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
          <span b-7882z672yd class="navbar-toggler-icon"></span>
        </button>
        <div b-7882z672yd class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
          <ul b-7882z672yd class="navbar-nav flex-grow-1">
            <li b-7882z672yd class="nav-item">
              <RouterLink to="/" class="nav-link text-dark" active-class="active"> Home</RouterLink>
            </li>
            <li b-7882z672yd class="nav-item">
              <RouterLink to="/association" class="nav-link text-dark" active-class="active"> Associations</RouterLink>
            </li>
            <li b-7882z672yd class="nav-item">
              <RouterLink to="/owner" class="nav-link text-dark" active-class="active"> Owners</RouterLink>
            </li>
            <li b-7882z672yd class="nav-item">
              <RouterLink to="/fund" class="nav-link text-dark" active-class="active"> Funds</RouterLink>
            </li>
          </ul>

          <ul class="navbar-nav">
            <template v-if="identityStore.$state.jwt?.token == undefined">
              <li class="nav-item">
                <RouterLink to="/identity/account/register" class="nav-link text-dark" active-class="active"> Register</RouterLink>
                
              </li>
              <li class="nav-item">
                <RouterLink to="/identity/account/login" class="nav-link text-dark" active-class="active"> Login</RouterLink>
                
              </li>
            </template>
            <template v-else>
              <li class="nav-item">
                <a class="nav-link text-dark" >{{identityStore.$state.jwt?.firstName}} {{identityStore.$state.jwt?.lastName}} </a>
              </li>
              <li class="nav-item">
                <a class="nav-link text-dark" href="/identity/account/login">Logout</a>
              </li>
            </template>
          </ul>
        </div>
      </div>
    </nav>
  </header>
</template>

<script lang="ts">
import { useIdentityStore } from "@/stores/Identity";
import { Options, Vue } from "vue-class-component";
import { RouterLink } from "vue-router";

@Options({
  components: {},
})
export default class Header extends Vue {
  identityStore = useIdentityStore();
}
</script>
