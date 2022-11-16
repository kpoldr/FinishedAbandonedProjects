import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { IAssociation } from "../../domain/IAssociation";
import { AssociationService } from "../../services/AssociationService";


const initialState: IAssociation[] = [];

function Association() {
    const associationService = new AssociationService();

    const [associations, setAssociations] = useState(initialState);

    useEffect(() => {
        associationService.getAll().then((data) => setAssociations(data));
    }, []);

    return (
        <>
            <h2>Associations</h2>
            <p>
                <Link to={`/Associations/Create/`}>Create New </Link>{" "}
            </p>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Email</th>
                        <th>Phone</th>
                        <th>Bank Name</th>
                        <th>Bank Number</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {associations.map((item) => {
                        return (
                            <tr key={item.id}>
                                <td>{item.id}</td>
                                <td>{item.name.en}</td>
                                <td>{item.description?.en}</td>
                                <td>{item.email}</td>
                                <td>{item.phone}</td>
                                <td>{item.bankName?.en}</td>
                                <td>{item.bankNumber}</td>
                                <td>
                                <Link to={`/Associations/Edit/${item.id}`}>Edit </Link> |{" "}
                                    
                                    <Link to={`/Associations/Details/${item.id}`}>
                                        Details{" "}
                                    </Link>{" "}
                                    |{" "}
                                    <Link to={`/Associations/Delete/${item.id}`}>
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

export default Association;
