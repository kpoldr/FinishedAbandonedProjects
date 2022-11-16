<template>
  <h1>Login</h1>

  <hr />
  <div class="row">
    <div class="col-md-4">
      <div v-if="errorMessage != null" class="text-danger validation-summary-errors" data-valmsg-summary="true">
        <ul>
          <li>{{ errorMessage }}</li>
        </ul>
      </div>
      <div>
        <div class="form-group">
          <label class="control-label" for="FirstName">Email</label>
          <input v-model="email" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" for="LastName">Password</label>
          <input v-model="password" class="form-control" type="password" />
        </div>
        <div class="form-group">
          <input @click="loginClicked()" type="submit" value="Login" class="btn btn-primary" />
        </div>
      </div>
    </div>
  </div>

  <div>
    <a href="/Persons">Back to List</a>
  </div>
</template>

<script lang="ts">
import { useIdentityStore } from "@/stores/Identity";
import { Options, Vue } from "vue-class-component";
import { IdentityService } from "@/service/IdentityService";
import jwt_decode from "jwt-decode";
@Options({
  components: {},
  props: {},
  emist: {},
})
export default class Login extends Vue {
  identityStore = useIdentityStore();
  email: string = "";
  password: string = "";
  errorMessage: string | null = null;

  IdentityService = new IdentityService();

  async loginClicked(): Promise<void> {
    console.log("submitclicked");
    var res = await this.IdentityService.login(this.email, this.password);
    console.log(res);
    if (res.status === 200) {
      await this.$router.push("/");

      this.identityStore.$state.jwt = res.data!;
      let jwt = jwt_decode(this.identityStore.$state.jwt.token);
      this.identityStore.$state.jwt.appUserId = jwt["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];
      console.log(this.identityStore.$state.jwt);
    } else {
    
    this.errorMessage = (res.status + " " + res.errorMessage)
    }

  }
}
</script>
