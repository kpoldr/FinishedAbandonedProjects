<template>
  <h1>Delete</h1>

  <h3>Are you sure you want to delete this?</h3>

  <div>
    <h4>Fund</h4>
    <hr />
    <dl class="row">
      <dt class="col-sm-2">Name</dt>
      <dd class="col-sm-10">{{ name }}</dd>
      <dt class="col-sm-2">Value</dt>
      <dd class="col-sm-10">{{ value }}</dd>
      <dt class="col-sm-2">Association</dt>
      <dd class="col-sm-10">{{ associationId }}</dd>
      
    </dl>
  </div>
  <div>
    <div>
      <div class="col-md-3 form-group btn-group">
        <input @click="deleteClicked()" type="submit" value="Delete" class="btn btn-danger p-2" />

        <RouterLink to="/fund" class="nav-link" active-class="active"> Back to List</RouterLink>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { FundService } from "@/service/FundService";
import { useFundStore } from "@/stores/fund";
import { useIdentityStore } from "@/stores/Identity";
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: { id: String },
  emist: {},
})
export default class FundDelete extends Vue {
  id!: string;
  identityStore = useIdentityStore();
  fundStore = useFundStore();
  fundService = new FundService();

  associations: string[] = [];

  name: string = "";
  value: number | null = null;
  associationId: string | null = null;


  async deleteClicked(): Promise<void> {
    console.log("Delete clicked");

    var res = await this.fundService.delete(this.id);

    if (res.status >= 300 && res.errorMessage) {
      this.errorMessages.push(res.status + " " + res.errorMessage);
    } else {
      this.fundStore.$state.funds = await this.fundService.getAll();

      await this.$router.push("/fund");
    }
  }

  async mounted() {
    console.log("mounted");
    let fund = await this.fundService.get(this.id);
    console.log(this.fundStore.$state.funds);
    if (fund != null) {
      this.name = fund["name"]["en"];
      this.value = fund["value"];
      this.associationId = fund["associationId"];
    }
  }
}
</script>
