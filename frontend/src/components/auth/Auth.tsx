import styles from './Auth.module.css'; 
import { FC } from 'react';
import { AuthProps } from './AuthTypes';
import LoginForm from './forms/LoginForm';
import RegisterForm from './forms/RegisterForm';

const Auth: FC<AuthProps> = (props: AuthProps) => {
    return (
        <div className={styles.formsAuthWrap}>
            <div className={styles.forms}>
                <div className={styles.toggleType}>
                    <button
                        onClick={() => { props.setToggle(true) }}
                        className={props.toggle ? styles.active : ''}
                    >
                        Логин
                    </button>
                    <button
                        onClick={() => { props.setToggle(false) }}
                        className={!props.toggle ? styles.active : ''}
                    >
                        Регистрация
                    </button>
                </div>
                <div className={styles.formsZone}>
                    {props.toggle ? (
                        <LoginForm
                            load={props.load}
                            login={props.login}
                            errorsLogin={props.errorsLogin}
                            submitLogin={props.submitLogin}
                            handleSubmitLogin={props.handleSubmitLogin}
                        />
                    ) : (
                        <RegisterForm
                            load={props.load}
                            register={props.register}
                            errors={props.errors}
                            handleSubmit={props.handleSubmit}
                            submitRegister={props.submitRegister}
                        />
                    )}
                </div>
            </div>
        </div>
    );

}

export default Auth;