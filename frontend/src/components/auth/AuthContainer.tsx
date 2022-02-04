import Auth from './Auth';
import { Navigate, useNavigate } from 'react-router';
import { useDispatch } from 'react-redux';
import { FC, useState } from 'react';
import { userLogin } from '../../redux/actions/authActions';
import { useForm } from 'react-hook-form';
import { authService } from '../../http/services/authService';
import { FormValues } from './AuthTypes';
import { AppDispatch } from '../../App';
import { useToasts } from 'react-toast-notifications';

const AuthContainer: FC = () => {

    const dispatch: AppDispatch = useDispatch();
    const navigate = useNavigate();
    const { addToast } = useToasts();
    const [toggle, setToggle] = useState(true);
    const [load, setLoad] = useState(false);

    const {
        reset,
        register,
        handleSubmit,
        formState: { errors } } = useForm<FormValues>();
    const {
        register: login,
        handleSubmit: handleSubmitLogin,
        formState: { errors: errorsLogin } } = useForm<FormValues>();

    const submitLogin = (data: FormValues) => {
        setLoad(true);
        dispatch(userLogin(data.email, data.password))
            .then(() => {
                navigate('/home');
                setLoad(false);
            })
            .catch((error: any) => {
                addToast(error.response.data.errorMessage, {
                    appearance: 'error'
                });
                setLoad(false);
            });
    }

    const submitRegister = async (data: FormValues) => {
        setLoad(true);
        try {
            await authService.register(data.email, data.login, data.password);
            setToggle(false);
            reset();
        } catch (error: any) {
            addToast(error.response.data.errorMessage, {
                appearance: 'error'
            });
        } finally {
            setLoad(false);
        }
    }

    if (localStorage.getItem('token')) {
        return <Navigate to="/home" />
    }

    return (
        <Auth
            load={load}
            toggle={toggle}
            setToggle={setToggle}
            errorsLogin={errorsLogin}
            login={login}
            handleSubmitLogin={handleSubmitLogin}
            errors={errors}
            register={register}
            handleSubmit={handleSubmit}
            submitLogin={submitLogin}
            submitRegister={submitRegister}
        />
    );

}

export default AuthContainer;