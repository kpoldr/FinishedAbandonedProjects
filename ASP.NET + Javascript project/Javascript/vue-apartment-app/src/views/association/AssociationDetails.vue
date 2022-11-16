<template>
  <h1>Details</h1>

  <div>
    <h4>Association</h4>
    <hr />
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
    <div class="btn-group ">
      
        <RouterLink :to="{ name: 'associationedit', params: { id: id } }" type="submit">Edit</RouterLink> |
      
        <RouterLink to="/association" type="submit"> Back to List</RouterLink>
      
    </div>
  </div>
</template>

<script lang="ts">
import { AssociationService } from "@/service/AssociationService";
import { useAssociationsStore } from "@/stores/association";
import { Options, Vue } from "vue-class-component";
import { RouterLink, RouterView } from "vue-router";

@Options({
  components: {},
  props: { id: String },
  emist: {},
})
export default class AssociationDetails extends Vue {
  associationStore = useAssociationsStore();
  associationService = new AssociationService();
  id!: string;
  appUserId: string = "";
  name: string = "";
  description: string | undefined = "";
  email: string = "";
  phone: number | null = null;
  bankname: string | undefined = "";
  banknumber: string | undefined = "";

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
