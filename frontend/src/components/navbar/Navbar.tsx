import './Navbar.css';
import { Link } from "react-router-dom";
import { IState } from "../../models/IState";
import { useDispatch, useSelector } from "react-redux";
import { userLogout } from "../../redux/actions/authActions";

const Navbar = () => {
    const dispatch = useDispatch();
    const email = useSelector((state: IState) => state.auth.user.email);

    const submitLogout = () => {
        dispatch(userLogout());
    }

    return (
        <div>
            <nav>
                <ul>
                    <li>
                        <Link to="/main">Главная</Link>
                    </li>
                    <li>
                        <Link to="/create">Создать</Link>
                    </li>
                    <li className="right">
                        <button onClick={submitLogout}>{email}</button>
                    </li>
                </ul>
            </nav>

        </div>
    );
}

export default Navbar;