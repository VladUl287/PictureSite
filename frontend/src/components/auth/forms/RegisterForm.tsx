import { FC } from "react";
import { RegisterFormProps } from "../AuthTypes";
import styles from '../Auth.module.css';

const RegisterForm: FC<RegisterFormProps> = (props: RegisterFormProps) => {
    return (
        <form onSubmit={props.handleSubmit(props.submitRegister)}>
            <div>
                <input type='text' className={props.errors.email && styles.validateError}
                    {...props.register("email", {
                        required: true,
                        maxLength: 150,
                        pattern: /^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])+$/i
                    })}
                    placeholder='email'
                />
                {props.errors.email &&
                    <span className={styles.textError}>
                        некорректный email
                    </span>}
            </div>
            <div>
                <input type='text' className={props.errors.login && styles.validateError}
                    {...props.register("login", {
                        required: true,
                        minLength: 6
                    })}
                    placeholder='логин'
                />
                {props.errors.login &&
                    <span className={styles.textError}>
                        не менее 6-ти символов
                    </span>}
            </div>
            <div>
                <input type='text' className={props.errors.password && styles.validateError}
                    {...props.register("password", {
                        required: true,
                        minLength: 8,
                        maxLength: 100,
                        pattern: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/i
                    })}
                    placeholder='пароль'
                />
                {props.errors.password &&
                    <span className={styles.textError}>
                        буквы латинского алфавита и цифры
                    </span>}
            </div>
            <button type='submit' disabled={props.load} >
                {props.load ? <span className={styles.loading}></span> : <span>Зарегистрироваться</span>}
            </button>
        </form>
    );
}

export default RegisterForm;