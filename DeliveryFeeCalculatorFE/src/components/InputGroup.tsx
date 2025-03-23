import React from "react";

interface InputGroupProps {
  label: string;
  type?: string;
  name?: string;
  value: string;
  onChange?: (e: React.ChangeEvent<HTMLInputElement>) => void;
  disabled?: boolean;
  placeholder?: string;
}

export const InputGroup: React.FC<InputGroupProps> = ({
  label,
  type = "text",
  name,
  value,
  onChange,
  disabled = false,
  placeholder,
}) => {
  return (
    <div>
      <label className="block text-gray-700 font-medium">{label}</label>
      <input
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        disabled={disabled}
        placeholder={placeholder}
        className="w-full border border-gray-300 px-3 py-2 rounded-md focus:ring focus:ring-blue-300"
      />
    </div>
  );
};