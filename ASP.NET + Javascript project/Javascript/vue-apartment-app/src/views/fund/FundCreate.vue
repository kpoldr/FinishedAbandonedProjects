<template>
  <h1>Create</h1>

  <h4>Fund</h4>
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
          <label class="control-label">Value</label>
          <input v-model="value" class="form-control valid" type="number" />
        </div>
        <div class="form-group">
          <label class="control-label" for="Fund_AssociationId">Association ID</label>
          <select v-model="associationId" class="form-control valid" >
            <option v-bind:value="association.id" v-for="association in associationStore.associations" :key="association.id">{{ association.name }}</option>
          </select>
        </div>
        <div class="form-group pt-2">
          <input @click="submitClicked()" type="submit" value="Create" class="btn btn-primary" />
        </div>
      </div>
    </div>
  </div>

  <div>
    <RouterLink to="/fund" class="nav-link" active-class="active"> Back to List</RouterLink>
  </div>
</template>

<script lang="ts">
import { useFundStore } from "@/stores/fund";
import { FundService } from "@/service/FundService";
import { Options, Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/Identity";
import { AssociationService } from "@/service/AssociationService";
import { useAssociationsStore } from "@/stores/association";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class FundCreate extends Vue {
  identityStore = useIdentityStore();
  fundStore = useFundStore();
  fundService = new FundService();

  associationService = new AssociationService();
  associationStore = useAssociationsStore();
  associations: string[] = [];
  errorMessages: string[] = [];

  name: string = "";
  value: number | null = null;
  associationId: string | null = null;

  async submitClicked(): Promise<void> {
    console.log("Register clicked");
    this.errorMessages = [];

    if (this.name === "") {
      this.errorMessages.push("Name cannot be empty");
    }

    if (this.value === null) {
      this.errorMessages.push("Value cannot be empty");
    }

    if (this.value === null) {
      this.errorMessages.push("Must select one association");
    }

    if (this.errorMessages.length === 0) {

      var res = await this.fundService.add({
        name: { en: this.name },
        value: this.value,
        associationId: this.associationId
      });

      if (res.status >= 300 && res.errorMessage) {
        this.errorMessages.push(res.status + " " + res.errorMessage);
      } else {
        this.fundStore.$state.funds = await this.fundService.getAll();

        await this.$router.push("/fund");
      }
    }
  }

  async mounted(): Promise<void> {
    this.associationStore.$state.associations = await this.associationService.getAll();
  }
}
</script>
