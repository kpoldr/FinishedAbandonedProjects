<template>
  <h1>Edit</h1>

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
          <label class="control-label" for="Association_Name">Name</label>
          <input v-model="name" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Association_Description">Description</label>
          <input v-model="description" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Association_Email">Email</label>
          <input v-model="email" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Association_Phone">Phone</label>
          <input v-model="phone" class="form-control valid" type="number" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Association_BankName">Bank name</label>
          <input v-model="bankname" class="form-control" type="text" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Association_BankNumber">Bank number</label>
          <input v-model="banknumber" class="form-control" type="text" />
        </div>

        <div class="form-group btn-group pt-3">
          <input @click="submitClicked()" type="submit" value="Edit" class="btn btn-primary" />
          <RouterLink to="/association" class="nav-link" active-class="active"> Back to List</RouterLink>
        </div>
        <div>
    
  </div>
      </div>
    </div>
  </div>

  
</template>

<script lang="ts">
import { useAssociationsStore } from "@/stores/association";
import { AssociationService } from "@/service/AssociationService";
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/Identity";

@Options({
  components: {},
  props: { id: String },
  emist: {},
})
export default class AssociationEdit extends Vue {
  id!: string;
  identityStore = useIdentityStore();
  associationStore = useAssociationsStore();
  associationService = new AssociationService();
  errorMessages: string[] = [];

  appUserId: string = "";
  name: string = "";
  description?: string = "";
  email: string = "";
  phone: number | null = null;
  bankname?: string = "";
  banknumber?: string = "";

  async submitClicked(): Promise<void> {
    console.log("submitclicked");

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

      var res = await this.associationService.update(this.id, {
        id: this.id,
        name: { "en": this.name },
        description: { "en": this.description },
        email: this.email,
        phone: this.phone,
        bankName: { "en": this.bankname },
        bankNumber: this.banknumber,
        appUserId: appUserId,
      });

      if (res.status >= 300 && res.errorMessage) {
        this.errorMessages.push(res.status + " " + res.errorMessage);
      } else {
        this.associationStore.$state.associations = await this.associationService.getAll();

        await this.$router.push("/association");
      }
    }
  }

  async mounted() {
    console.log("mounted");
    let association = await this.associationService.get(this.id);
    console.log(this.associationStore.$state.associations);
    if (association != null) {
      this.name = association["name"]["en"];
      this.description = association["description"]? association["description"]["en"] : undefined;
      this.email = association["email"];
      this.phone = association["phone"];
      this.bankname = association["bankName"]? association["bankName"]["en"] : undefined;
      this.banknumber = association["bankNumber"];
    }
  }
}
</script>
