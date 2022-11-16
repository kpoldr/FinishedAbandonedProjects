<template>
  <h1>Delete</h1>

  <h3>Are you sure you want to delete this?</h3>

  <h4>Owner</h4>
  <hr />
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
    <div>
      <div class="col-md-3 form-group btn-group">
        <input @click="deleteClicked()" type="submit" value="Delete" class="btn btn-danger p-2" />

        <RouterLink to="/association" class="nav-link" active-class="active"> Back to List</RouterLink>
      </div>
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
export default class OwnerDelete extends Vue {
  id!: string;
  identityStore = useIdentityStore();
  ownerStore = useOwnerStore();
  ownerService = new OwnerService();
  errorMessages: string[] = [];

  name: string = "";
  email: string = "";
  phone: number | null = null;
  advancedPay: number | null = null;

  async deleteClicked(): Promise<void> {
    console.log("Delete clicked");

    var res = await this.ownerService.delete(this.id);

    if (res.status >= 300 && res.errorMessage) {
      this.errorMessages.push(res.status + " " + res.errorMessage);
    } else {
      this.ownerStore.$state.owners = await this.ownerService.getAll();

      await this.$router.push("/owner");
    }
  }

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
