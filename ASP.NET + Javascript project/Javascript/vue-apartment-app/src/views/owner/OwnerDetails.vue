<template>
  <h1>Details</h1>

  <div>
    <h4>Owner</h4>
    <hr />
    <dl class="row">
      <dt class="col-sm-2">Name</dt>
      <dd class="col-sm-10">{{ name }}</dd>
      <dt class="col-sm-2">Email</dt>
      <dd class="col-sm-10">{{ email }}</dd>
      <dt class="col-sm-2">Phone</dt>
      <dd class="col-sm-10">{{ phone }}</dd>
      <dt class="col-sm-2">Advanced Pay</dt>
      <dd class="col-sm-10">{{ advancedPay }}</dd>
    </dl>
  </div>
  <div>
    <div class="btn-group ">
      
        <RouterLink :to="{ name: 'owneredit', params: { id: id } }" type="submit">Edit</RouterLink> |
      
        <RouterLink to="/owner" type="submit"> Back to List</RouterLink>
      
    </div>
  </div>
</template>

<script lang="ts">
import { OwnerService } from "@/service/OwnerService";
import { useIdentityStore } from "@/stores/Identity";
import { useOwnerStore } from "@/stores/owner";
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: { id: String },
  emist: {},
})
export default class OwnerDetials extends Vue {
  id!: string;
  identityStore = useIdentityStore();
  ownerStore = useOwnerStore();
  ownerService = new OwnerService();
  errorMessages: string[] = [];

  name: string = "";
  email: string = "";
  phone: number | null = null;
  advancedPay: number | null = null;;

  async mounted() {
    console.log("mounted");
    let owner = await this.ownerService.get(this.id);
    console.log(this.ownerStore.$state.owners);
    if (owner != null) {
      this.name = owner["name"]["en"];
      this.email = owner["email"];
      this.phone = owner["phone"];
      this.advancedPay = owner["advancedPay"];
    }
  }
}
</script>
