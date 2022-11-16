<template>
  <main b-7882z672yd="" role="main" class="pb-3">
    <h1>Index</h1>

    <p>
      <RouterLink to="/association/create">Create new </RouterLink>
    </p>
    <table class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Email</th>
          <th>Phone</th>
          <th>Bank name</th>
          <th>Bank number</th>

          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item of associationStore.associations" :key="item.id">
          <td>{{item.name}}</td>
          <td>{{item.description}}</td>
          <td>{{item.email}}</td>
          <td>{{item.phone}}</td>
          <td>{{item.bankName}}</td>
          <td>{{item.bankNumber}}</td>
          
          <td>
            <RouterLink :to="{ name: 'associationedit', params: { id: item.id } }">Edit </RouterLink>|
            <RouterLink :to="{ name: 'associationdetails', params: { id: item.id } }">Details </RouterLink>|
            <RouterLink :to="{ name: 'associationdelete', params: { id: item.id } }">Delete </RouterLink> |
          </td>
        </tr>
      </tbody>
    </table>
  </main>
</template>

<script lang="ts">
import { useAssociationsStore } from "@/stores/association";
import { AssociationService } from "@/service/AssociationService"
import { Options, Vue } from "vue-class-component";

@Options({
  components: {},
  props: {},
  emist: {},
})
export default class AssociationIndex extends Vue {

    associationStore = useAssociationsStore();
    associationService = new AssociationService()

     async mounted(): Promise<void> {
    
    this.associationStore.$state.associations = await this.associationService.getAll();

  }


}
</script>
