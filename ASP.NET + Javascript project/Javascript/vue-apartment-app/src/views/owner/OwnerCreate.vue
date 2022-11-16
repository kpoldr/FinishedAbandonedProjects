<template>
  <h1>Create</h1>

  <h4>Owner</h4>
  <hr />
  <div>
    <ul>
      <li class="text-danger" v-for="error in errorMessages" :key="error">
        {{ error }}
      </li>
    </ul>
  </div>
  <div class="row">
    <div class="col-md-4">
      <div>
        <div class="form-group">
          <label class="control-label">Name</label>
          <input v-model="name" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label">Email</label>
          <input v-model="email" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label">Phone</label>
          <input v-model="phone" class="form-control valid" type="number" />
        </div>
        <div class="form-group">
          <label class="control-label">Advanced Pay</label>
          <input v-model="advancedPay" class="form-control" type="number" />
        </div>

        <div class="form-group pt-2">
          <input @click="submitClicked()" type="submit" value="Create" class="btn btn-primary " />
        </div>
      </div>
    </div>
  </div>

  <div>
    <RouterLink to="/owner" class="nav-link" active-class="active"> Back to List</RouterLink>
  </div>
</template>

<script lang="ts">
import { useOwnerStore } from "@/stores/owner";
import { OwnerService } from "@/service/OwnerService";
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/Identity";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class OwnerCreate extends Vue {
  identityStore = useIdentityStore();
  ownerStore = useOwnerStore();
  ownerService = new OwnerService();
  errorMessages: string[] = [];

  name: string = "";
  email: string = "";
  phone: number | null = null;
  advancedPay: number | null = null;;

  async submitClicked(): Promise<void> {
    console.log("Register clicked");
    this.errorMessages = [];

    if (this.name === "") {
      this.errorMessages.push("Name cannot be empty");
    }

    if (this.phone === null) {
      this.errorMessages.push("Phone number cannot be empty");
    }

    if (this.advancedPay === null) {
      this.errorMessages.push("Advanced cannot be empty");
    }

    if (this.email.length < 6) {
      this.errorMessages.push("Email must be longer than 5");
    }

    

    if (this.errorMessages.length === 0) {
      let appUserId = this.identityStore.$state.jwt!.appUserId;

      if (appUserId === null) {
        await this.$router.push("/");
        return;
      }

      var res = await this.ownerService.add({
        name: {"en": this.name},
        email: this.email,
        phone: this.phone!,
        advancedPay: this.advancedPay!,
        
      });


      if (res.status >= 300 && res.errorMessage) {
        this.errorMessages.push(res.status + " " + res.errorMessage);
      } else {

        this.ownerStore.$state.owners = await this.ownerService.getAll();

        await this.$router.push("/owner");
      }
    }
  }

  async mounted(): Promise<void> {
    this.ownerStore.$state.owners = await this.ownerService.getAll();
  }
}
</script>
