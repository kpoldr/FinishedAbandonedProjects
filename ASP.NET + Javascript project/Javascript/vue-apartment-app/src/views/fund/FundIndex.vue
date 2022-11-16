<template>
  <main b-7882z672yd="" role="main" class="pb-3">
    <h1>Index</h1>

    <p>
      <RouterLink to="/fund/create">Create new </RouterLink>
    </p>
    <table class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Value</th>

          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item of fundStore.funds" :key="item.id">
          <td>{{ item.name }}</td>
          <td>{{ item.value }}</td>
          <td>
            <RouterLink :to="{ name: 'fundedit', params: { id: item.id } }">Edit </RouterLink>| <RouterLink :to="{ name: 'funddetails', params: { id: item.id } }">Details </RouterLink>|
            <RouterLink :to="{ name: 'funddelete', params: { id: item.id } }">Delete </RouterLink> |
          </td>
        </tr>
      </tbody>
    </table>
  </main>
</template>

<script lang="ts">
import { useFundStore } from "@/stores/fund";
import { FundService } from "@/service/FundService";
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class FundIndex extends Vue {
  fundStore = useFundStore();
  fundService = new FundService();

  async mounted(): Promise<void> {
    this.fundStore.$state.funds = await this.fundService.getAll();
  }
}
</script>
