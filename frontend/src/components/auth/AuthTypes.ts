import { FieldErrors, SubmitHandler, UseFormHandleSubmit, UseFormRegister } from "react-hook-form"

export type FormValues = {
    email: string;
    login: string;
    password: string;
    formError: string;
}

export type AuthProps = {
    load: boolean;
    toggle: boolean;
    setToggle: Function;
    login: UseFormRegister<FormValues>;
    errorsLogin: FieldErrors;
    handleSubmitLogin: UseFormHandleSubmit<FormValues>;
    register: UseFormRegister<FormValues>;
    errors: FieldErrors;
    handleSubmit: UseFormHandleSubmit<FormValues>;
    submitLogin: SubmitHandler<FormValues>;
    submitRegister: SubmitHandler<FormValues>;
}

export type LoginFormProps = {
    load: boolean;
    login: UseFormRegister<FormValues>;
    errorsLogin: FieldErrors;
    handleSubmitLogin: UseFormHandleSubmit<FormValues>;
    submitLogin: SubmitHandler<FormValues>;
}

export type RegisterFormProps = {
    load: boolean;
    register: UseFormRegister<FormValues>;
    errors: FieldErrors;
    handleSubmit: UseFormHandleSubmit<FormValues>;
    submitRegister: SubmitHandler<FormValues>;
}