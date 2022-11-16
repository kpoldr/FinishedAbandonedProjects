<template>
  <main b-7882z672yd="" role="main" class="pb-3">
    <h1>Index</h1>

    <p>
      <RouterLink to="/owner/create">Create new </RouterLink>
    </p>
    <table class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Email</th>
          <th>Phone</th>
          <th>Advanced pay</th>

          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item of ownerStore.owners" :key="item.id">
          <td>{{ item.name }}</td>
          <td>{{ item.email }}</td>
          <td>{{ item.phone }}</td>
          <td>{{ item.advancedPay }}</td>

          <td>
            <RouterLink :to="{ name: 'owneredit', params: { id: item.id } }">Edit </RouterLink>| <RouterLink :to="{ name: 'ownerdetails', params: { id: item.id } }">Details </RouterLink>|
            <RouterLink :to="{ name: 'ownerdelete', params: { id: item.id } }">Delete </RouterLink> |
          </td>
        </tr>
      </tbody>
    </table>
  </main>
</template>

<script lang="ts">
import { useOwnerStore } from "@/stores/owner";
import { OwnerService } from "@/service/OwnerService";
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class FundIndex extends Vue {
  ownerStore = useOwnerStore();
  ownerService = new OwnerService();

  async mounted(): Promise<void> {
    this.ownerStore.$state.owners = await this.ownerService.getAll();
  }
}
</script>
