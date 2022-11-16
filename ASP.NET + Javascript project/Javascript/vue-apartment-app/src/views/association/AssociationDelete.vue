<template>
  <h1>Delete</h1>

  <h3>Are you sure you want to delete this?</h3>
  <div>
    <h4>Association</h4>
    <hr />
    <div>
      <ul>
        <li class="text-danger" v-for="error in errorMessages" :key="error">
          {{ error }}
        </li>
      </ul>
    </div>
    <dl class="row">
      <dt class="col-sm-2">Name</dt>
      <dd class="col-sm-10">{{ name }}</dd>
      <dt class="col-sm-2">Description</dt>
      <dd class="col-sm-10">{{ description }}</dd>
      <dt class="col-sm-2">Email</dt>
      <dd class="col-sm-10">{{ email }}</dd>
      <dt class="col-sm-2">Phone</dt>
      <dd class="col-sm-10">{{ phone }}</dd>
      <dt class="col-sm-2">Bank name</dt>
      <dd class="col-sm-10">{{ bankname }}</dd>
      <dt class="col-sm-2">Bank number</dt>
      <dd class="col-sm-10">{{ banknumber }}</dd>
    </dl>
  </div>
  <div>
    <div>
      <div class="col-md-3 form-group btn-group">
        
        
        <input @click="deleteClicked()" type="submit" value="Delete" class="btn btn-danger p-2" /> 
        
        
        <RouterLink to="/association" class="nav-link" active-class="active"> Back to List</RouterLink>
            
        
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { AssociationService } from "@/service/AssociationService";
import { useAssociationsStore } from "@/stores/association";
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: { id: String },
  emist: {},
})
export default class AssociationDetails extends Vue {
  associationStore = useAssociationsStore();
  associationService = new AssociationService();
  errorMessages = [];
  id!: string;
  appUserId: string = "";
  name: string = "";
  description: string | undefined = "";
  email: string = "";
  phone: number | null = null;
  bankname: string | undefined = "";
  banknumber: string | undefined = "";

  async deleteClicked(): Promise<void> {
    console.log("Delete clicked");

    var res = await this.associationService.delete(this.id);

    if (res.status >= 300 && res.errorMessage) {
      this.errorMessages.push(res.status + " " + res.errorMessage);
    } else {
      this.associationStore.$state.associations = await this.associationService.getAll();

      await this.$router.push("/association");
    }
  }

  async mounted() {
    console.log("mounted");
    let association = await this.associationService.get(this.id);
    console.log(this.associationStore.$state.associations);
    if (association != null) {
      this.name = association["name"];
      this.description = association["description"];
      this.email = association["email"];
      this.phone = association["phone"];
      this.bankname = association["bankName"];
      this.banknumber = association["bankNumber"];
    }
  }
}
</script>
