import Header from "./components/Header";
import Footer from "./components/Footer";
import Home from "./components/Home";
import { Route, Routes } from "react-router-dom";
import Page404 from "./components/Page404";
import Register from "./views/identity/Register";
import Login from "./views/identity/Login";
import Association from "./views/associations/Association";
import AssociationCreate from "./views/associations/AssociationCreate";
import AssociationEdit from "./views/associations/AssociationEdit";
import AssociationDetails from "./views/associations/AssociationDetails";
import AssociationDelete from "./views/associations/AssociationDelete";
import Owner from "./views/owner/Owner";
import OwnerCreate from "./views/owner/OwnerCreate";
import OwnerEdit from "./views/owner/OwnerEdit";
import OwnerDetails from "./views/owner/OwnerDetails";
import OwnerDelete from "./views/owner/OwnerDelete";
import Fund from "./views/fund/Fund";
import FundCreate from "./views/fund/FundCreate";
import FundEdit from "./views/fund/FundEdit";
import FundDetails from "./views/fund/FundDetails";
import FundDelete from "./views/fund/FundDelete";



function App() {

    return (
        <div className="container vh-100">
            
            <Header />
            <main role="main" className="pb-3 h-100">
                <div >
                    <Routes>
                        <Route path="/" element={<Home />} />
                        <Route path="/Associations" element={<Association />} />
                        <Route path="/Associations/Create" element={<AssociationCreate />} />
                        <Route path="/Associations/Edit/:id" element={<AssociationEdit />} />
                        <Route path="/Associations/Details/:id" element={<AssociationDetails />} />
                        <Route path="/Associations/Delete/:id" element={<AssociationDelete />} />
                        <Route path="/Owners" element={<Owner />} />
                        <Route path="/Owners/Create" element={<OwnerCreate />} />
                        <Route path="/Owners/Edit/:id" element={<OwnerEdit />} />
                        <Route path="/Owners/Details/:id" element={<OwnerDetails />} />
                        <Route path="/Owners/Delete/:id" element={<OwnerDelete />} />
                        <Route path="/Funds" element={<Fund />} />
                        <Route path="/Funds/Create" element={<FundCreate />} />
                        <Route path="/Funds/Edit/:id" element={<FundEdit />} />
                        <Route path="/Funds/Details/:id" element={<FundDetails />} />
                        <Route path="/Funds/Delete/:id" element={<FundDelete />} />
                        <Route path="/Login" element={<Login />} />
                        <Route path="/Register" element={<Register />} />
                        <Route path="*" element={<Page404 />} />
                    </Routes>
                </div>
            </main>
            <Footer />
            
        </div>
    );
}

export default App;
