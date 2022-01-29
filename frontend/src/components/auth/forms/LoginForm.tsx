import { FC } from "react";
import { LoginFormProps } from "../AuthTypes";
import styles from '../Auth.module.css';

const LoginForm: FC<LoginFormProps> = (props: LoginFormProps) => {
    return (
        <form onSubmit={props.handleSubmitLogin(props.submitLogin)}>
            <div>
                <input type='text' className={props.errorsLogin.email && styles.validateError}
                    {...props.login("email", {
                        required: true,
                        maxLength: 150
                    })}
                    placeholder='email'
                />
                {props.errorsLogin.email &&
                    <span className={styles.textError}>
                        обязательное поле
                    </span>}
            </div>
            <div>
                <input type='text' className={props.errorsLogin.password && styles.validateError}
                    {...props.login("password", {
                        required: true,
                        maxLength: 150,
                    })}
                    placeholder='пароль'
                />
                {props.errorsLogin.password &&
                    <span className={styles.textError}>
                        обязательное поле
                    </span>}
            </div>
            <button type='submit' disabled={props.load} >
                {props.load ? <span className={styles.loading}></span> : <span>Войти</span>}
            </button>
        </form>
    );
}

export default LoginForm;