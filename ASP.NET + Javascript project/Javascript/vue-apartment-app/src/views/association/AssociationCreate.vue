<template>
  <h1>Create</h1>

  <h4>Association</h4>
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
          <label class="control-label">Description</label>
          <input v-model="description" class="form-control" type="text" />
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
          <label class="control-label" >Bank name</label>
          <input v-model="bankname" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" >Bank number</label>
          <input v-model="banknumber" class="form-control" type="text" />
        </div>

        <div class="form-group pt-1">
          <input @click="submitClicked()" type="submit" value="Create" class="btn btn-primary" />
        </div>
      </div>
    </div>
  </div>

  <div>
    <RouterLink to="/association" class="nav-link" active-class="active"> Back to List</RouterLink>
  </div>
</template>

<script lang="ts">
import { useAssociationsStore } from "@/stores/association";
import { AssociationService } from "@/service/AssociationService";
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/Identity";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class AssociationCreate extends Vue {
  identityStore = useIdentityStore();
  associationStore = useAssociationsStore();
  associationService = new AssociationService();
  errorMessages: string[] = [];

  name: string = "";
  description: string = "";
  email: string = "";
  phone: number | null = null;
  bankname: string = "";
  banknumber: string = "";

  async submitClicked(): Promise<void> {
    console.log("Register clicked");
    this.errorMessages = [];

    if (this.name === "") {
      this.errorMessages.push("Name cannot be empty");
    }

    if (this.phone === null) {
      this.errorMessages.push("Phone number cannot be empty");
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

      var res = await this.associationService.add({
        name: {"en": this.name},
        description: {"en": this.description},
        email: this.email,
        phone: this.phone,
        bankName: {"en":this.bankname},
        bankNumber: this.banknumber,
        appUserId: appUserId
      });


      if (res.status >= 300 && res.errorMessage) {
        this.errorMessages.push(res.status + " " + res.errorMessage);
      } else {

        this.associationStore.$state.associations = await this.associationService.getAll();

        await this.$router.push("/association");
      }
    }
  }

  async mounted(): Promise<void> {
    this.associationStore.$state.associations = await this.associationService.getAll();
  }
}
</script>
