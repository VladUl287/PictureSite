import { AnyAction } from 'redux';
import AppRoutes from './AppRoutes';
import { IState } from './models/IState';
import { useDispatch, useSelector } from 'react-redux';
import { ThunkDispatch } from 'redux-thunk';
import { FC, useEffect, useState } from 'react';
import Navbar from './components/navbar/Navbar';
import { checkAuth } from './redux/actions/authActions';

export type AppDispatch = ThunkDispatch<IState, any, AnyAction>

const App: FC = () => {
  const dispatch: AppDispatch = useDispatch();
  const isAuth = useSelector((state: IState) => state.auth.isAuth);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (localStorage.getItem('token')) {
      dispatch(checkAuth())
        .then(() => setLoading(false))
        .catch(() => setLoading(false));
    } else {
      setLoading(false)
    }
  }, []);

  if (loading) {
    return <div>Loading app...</div>
  }

  return (
    <div className="App">
      {/* {isAuth && <Navbar />} */}
      <Navbar />
      <AppRoutes isAuth />
    </div>
  );
}

export default App;