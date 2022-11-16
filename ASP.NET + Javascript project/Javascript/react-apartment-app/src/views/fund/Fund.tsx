import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { IFund } from "../../domain/IFund";
import { AssociationService } from "../../services/AssociationService";
import { FundService } from "../../services/FundService";


const initialFundState: IFund[] = [];
const initialAssociationState: IAssociation[] = [];

function Fund() {
    const fundService = new FundService();
    const associationService = new AssociationService();

    const [funds, setFunds] = useState(initialFundState);
    const [associations, setAssociations] = useState(initialAssociationState);
    
    useEffect(() => {
        fundService.getAll().then((data) => setFunds(data));
        associationService.getAll().then((data) => setAssociations(data));
    }, []);

    return (
        <>
            <h2>Funds</h2>
            <p>
                <Link to={`/Funds/Create/`}>Create New </Link>{" "}
            </p>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>Name</th>
                        <th>Value</th>
                        <th>Association</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    
                    {
                        funds.map((item) => {
                            return (
                                <tr key={item.id}>
                                    <td>{item.id}</td>
                                    <td>{item.name.en}</td>
                                    <td>{item.value}</td>
                                    {(associations !== undefined) &&
                                        <td>{associations.find(a => a.id === item.associationId)?.name.en}</td>
                                    }
                                    <td>
                                    <Link to={`/Funds/Edit/${item.id}`}>Edit </Link> |{" "}
                                        
                                        <Link to={`/Funds/Details/${item.id}`}>
                                            Details{" "}
                                        </Link>{" "}
                                        |{" "}
                                        <Link to={`/Funds/Delete/${item.id}`}>
                                            Delete{" "}
                                        </Link>
                                    </td>
                                </tr>
                            );
                        })}
                    
                    
                
                
                    
                </tbody>
            </table>
        </>
    );
}

export default Fund;
