<template>
  <h1>Details</h1>

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
    <div class="btn-group ">
      
        <RouterLink :to="{ name: 'fundedit', params: { id: id } }" type="submit">Edit</RouterLink> |
      
        <RouterLink to="/fund" type="submit"> Back to List</RouterLink>
      
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
export default class FundDetials extends Vue {
  id!: string;
  identityStore = useIdentityStore();
  fundStore = useFundStore();
  fundService = new FundService();

  associations: string[] = [];

  name: string = "";
  value: number | null = null;
  associationId: string | null = null;

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
