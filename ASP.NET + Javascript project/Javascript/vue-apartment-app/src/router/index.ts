import Login from "@/views/identity/Login.vue";
import Register from "@/views/identity/Register.vue";
import AssociationCreate from "@/views/association/AssociationCreate.vue";
import AssociationDelete from "@/views/association/AssociationDelete.vue";
import AssociationDetails from "@/views/association/AssociationDetails.vue";
import AssociationEdit from "@/views/association/AssociationEdit.vue";
import AssociationIndex from "@/views/association/AssociationIndex.vue";
import OwnerCreate from "@/views/owner/OwnerCreate.vue";
import OwnerDelete from "@/views/owner/OwnerDelete.vue";
import OwnerDetails from "@/views/owner/OwnerDetails.vue";
import OwnerEdit from "@/views/owner/OwnerEdit.vue";
import OwnerIndex from "@/views/owner/OwnerIndex.vue";
import FundCreate from "@/views/fund/FundCreate.vue";
import FundDelete from "@/views/fund/FundDelete.vue";
import FundDetails from "@/views/fund/FundDetails.vue";
import FundEdit from "@/views/fund/FundEdit.vue";
import FundIndex from "@/views/fund/FundIndex.vue";
import { createRouter, createWebHistory } from "vue-router";
import HomeView from "../views/HomeView.vue";

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: "/",
      name: "home",
      component: HomeView,
    },
    {path: "/identity/account/login", name: "login", component: Login },
    {path: "/identity/account/register", name: "register", component: Register },
    {path: "/fund", name: "fundindex", component: FundIndex },
    {path: "/fund/create", name: "fundcreate", component: FundCreate },
    {path: "/fund/edit/:id", name: "fundedit", component: FundEdit, props: true },
    {path: "/fund/delete/:id", name: "funddelete", component: FundDelete, props: true },
    {path: "/fund/details/:id", name: "funddetails", component: FundDetails, props: true },
    {path: "/owner", name: "ownerindex", component: OwnerIndex },
    {path: "/owner/create", name: "ownercreate", component: OwnerCreate },
    {path: "/owner/edit/:id", name: "owneredit", component: OwnerEdit, props: true },
    {path: "/owner/details/:id", name: "ownerdetails", component: OwnerDetails, props: true },
    {path: "/owner/delete/:id", name: "ownerdelete", component: OwnerDelete, props: true },
    {path: "/association", name: "associationindex", component: AssociationIndex },
    {path: "/association/create", name: "associationcreate", component: AssociationCreate },
    {path: "/association/edit/:id", name: "associationedit", component: AssociationEdit, props: true },
    {path: "/association/details/:id", name: "associationdetails", component: AssociationDetails, props: true },
    {path: "/association/delete/:id", name: "associationdelete", component: AssociationDelete, props: true },
  ],
});

export default router;
