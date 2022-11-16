<template>
  <div class="col-md-8">
    <h1>Register</h1>

    <hr />
    <div class="row">
      <div class="col-md-6">
        <div>
          <ul>
            <li class="text-danger" v-for="error in errorMessages" :key="error">
              {{ error }}
            </li>
          </ul>
        </div>

        <div>
          <div class="form-group">
            <label class="control-label" for="FirstName">Email</label>
            <input v-model="email" class="form-control" type="text" />
          </div>
          <div class="row">
            <div class="col">
              <div class="form-group">
                <label class="control-label" for="FirstName">First Name</label>
                <input v-model="firstName" class="form-control" type="text" />
              </div>
            </div>
            <div class="col">
              <div class="form-group">
                <label class="control-label" for="FirstName">Last Name</label>
                <input v-model="lastName" class="form-control" type="text" />
              </div>
            </div>
          </div>
          <div class="form-group">
            <label class="control-label" for="LastName">Password</label>
            <input v-model="password" class="form-control" type="password" />
          </div>
          <div class="form-group">
            <label class="control-label" for="LastName">Confirm Password</label>
            <input v-model="confirmPassword" class="form-control" type="password" />
          </div>
          <div class="form-group pt-3">
            <input @click="registerClicked()" type="submit" value="Create" class="btn btn-primary" />
          </div>
        </div>
      </div>
    </div>

    <div>
      <a href="/Persons">Back to List</a>
    </div>
  </div>
</template>

<script lang="ts">
import { useIdentityStore } from "@/stores/Identity";
import { Options, Vue } from "vue-class-component";
import { IdentityService } from "@/service/IdentityService";
@Options({
  components: {},
  props: {},
  emist: {},
})
export default class Register extends Vue {
  identityStore = useIdentityStore();
  email: string = "";
  password: string = "";
  confirmPassword: string = "";
  firstName: string = "";
  lastName: string = "";
  errorMessages: string[] = [];

  IdentityService = new IdentityService();

  async registerClicked(): Promise<void> {
    console.log("Register clicked");
    this.errorMessages = [];

    if (this.password != this.confirmPassword) {
      this.errorMessages.push("Password don't match");
    }

    if (this.password.length < 6) {
      this.errorMessages.push("Password must be longer than 6");
    }

    if (this.email.length < 6) {
      this.errorMessages.push("Email must be longer than 5");
    }

    if (this.firstName === "") {
      this.errorMessages.push("First name cannot be empty");
    }

    if (this.firstName === "") {
      this.errorMessages.push("Last name cannot be empty");
    }

    if (this.errorMessages.length === 0) {
      var res = await this.IdentityService.register(this.email, this.password, this.firstName, this.lastName);

      if (res.status >= 300 && res.errorMessage) {
        this.errorMessages.push(res.status + " " + res.errorMessage);
      } else {
        var res = await this.IdentityService.login(this.email, this.password);

        if (res.status === 200) {
          await this.$router.push("/home");
          this.identityStore.$state.jwt = res.data!;
        }
      }
    }
  }
}
</script>
