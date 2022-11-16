import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { IOwner } from "../../domain/IOwner";
import { OwnerService } from "../../services/OwnerService";


const initialState: IOwner[] = [];

function Owner() {
    const ownerService = new OwnerService();

    const [owners, setOwner] = useState(initialState);

    useEffect(() => {
        
        ownerService.getAll().then((data) => setOwner(data));
    }, []);

    return (
        <>
            <h2>Owners</h2>
            <p>
                <Link to={`/Owners/Create/`}>Create New </Link>{" "}
            </p>
            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>id</th>
                        <th>Name</th>
                        <th>Email</th>
                        <th>Phone</th>
                        <th>Advanced Pay</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {owners.map((item) => {
                        return (
                            <tr key={item.id}>
                                <td>{item.id}</td>
                                <td>{item.name.en}</td>
                                <td>{item.email}</td>
                                <td>{item.phone}</td>
                                <td>{item.advancedPay}</td>
                                <td>
                                <Link to={`/Owners/Edit/${item.id}`}>Edit </Link> |{" "}
                                    
                                    <Link to={`/Owners/Details/${item.id}`}>
                                        Details{" "}
                                    </Link>{" "}
                                    |{" "}
                                    <Link to={`/Owners/Delete/${item.id}`}>
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

export default Owner;
