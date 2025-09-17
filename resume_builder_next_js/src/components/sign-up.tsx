'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import styles from '../css/auth.module.css';
import  Loading  from './loading';

interface SignUpFormData {
  email: string;
  password: string;
  confirmPassword: string;
  passKey: string;
}

interface SignUpErrors {
  email?: string;
  password?: string;
  confirmPassword?: string;
  general?: string;
  passKey?: string;
}

interface SignUpProps {
  onToggleMode: () => void;
}

const SignUp: React.FC<SignUpProps> = ({ onToggleMode }) => {
  const router = useRouter();
  const [formData, setFormData] = useState<SignUpFormData>({
    email: '',
    password: '',
    confirmPassword: '',
    passKey: '',
  });

  const [errors, setErrors] = useState<SignUpErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const validateEmail = (email: string): boolean => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };

  const validatePassword = (password: string): boolean => {
    // At least 8 characters, one uppercase, one lowercase, one number
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d@$!%*?&]{8,}$/;
    return passwordRegex.test(password);
  };

  const validateForm = (): boolean => {
    const newErrors: SignUpErrors = {};

    // Email validation
    if (!formData.email) {
      newErrors.email = 'Email is required';
    } else if (!validateEmail(formData.email)) {
      newErrors.email = 'Please enter a valid email address';
    }

    // Password validation
    if (!formData.password) {
      newErrors.password = 'Password is required';
    } else if (!validatePassword(formData.password)) {
      newErrors.password = 'Password must be at least 8 characters with uppercase, lowercase, and number';
    }

    // Confirm password validation
    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'Please confirm your password';
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Passwords do not match';
    }

    // Pass Key validation
    if (!formData.passKey) {
      newErrors.general = 'Pass Key is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));

    // Clear specific error when user starts typing
    if (errors[name as keyof SignUpErrors]) {
      setErrors(prev => ({
        ...prev,
        [name]: undefined
      }));
    }
  };

  if (isSubmitting) {
    return <Loading />;
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);

    try{
      const resumeResponse = await fetch('./api/resume-api/User/Auth', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          path: 'Users/Auth',
          passKey: formData.passKey,
          Email: formData.email
        }),
      });

      if (!resumeResponse.ok) {
        const errorData = await resumeResponse.json();
        setErrors({ general: errorData.error || 'Resume API error occurred. Please try again.' });
        setIsSubmitting(false);
        return;
      }

      const resumeResult = await resumeResponse.json();

      console.log('Resume API result:', resumeResult);

    }
    catch (error) {
      setErrors({ general: 'Resume API error occurred. Please try again.' });
      setIsSubmitting(false);
      return;
    }
    
    try {
      const response = await fetch('./api/signup', {
        method: 'POST', 
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: formData.email,
          password: formData.password
        }),
      });

      const result = await response.json();

      if (!result.success) {
        if (result.message === 'Email already exists') {
          setErrors({ email: result.message });
        } else {
          setErrors({ general: result.message });
        }
        return;
      }

      // Success
      alert('Account created successfully!');
      
      // Redirect to dashboard with email parameter
      router.push(`/dashboard`);
      
      // Reset form
      setFormData({
        email: '',
        password: '',
        confirmPassword: '',
        passKey: '',
      });
      
    } catch (error) {
      setErrors({ general: 'Failed to create account. Please try again.' });
    } finally {
      setIsSubmitting(false);
    }
  };


  const togglePasswordVisibility = (field: 'password' | 'confirmPassword') => {
    if (field === 'password') {
      setShowPassword(!showPassword);
    } else {
      setShowConfirmPassword(!showConfirmPassword);
    }
  };

  return (
    <div className={styles.authContainer}>
      <div className={styles.authForm}>
        <h2 className={styles.authTitle}>Create Account</h2>
        
        {errors.general && (
          <div className={styles.errorMessage}>
            {errors.general}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          {/* Pass Key */}
          <div className={styles.fieldGroup}>
            <label htmlFor="passKey" className={styles.label}>
              Pass Key *
            </label>
            <input
              type="text"
              id="passKey"
              name="passKey"
              value={formData.passKey}
              onChange={handleInputChange}
              className={`purple-input ${styles.input}`}
              placeholder="Enter your pass key"
              disabled={isSubmitting}
            />
            {errors.passKey && (
              <p className={styles.errorText}>
                {errors.passKey}
              </p>
            )}
          </div>

          {/* Email Field */}
          <div className={styles.fieldGroup}>
            <label htmlFor="email" className={styles.label}>
              Email *
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              className={`purple-input ${styles.input}`}
              placeholder="Enter your email"
              disabled={isSubmitting}
            />
            {errors.email && (
              <p className={styles.errorText}>
                {errors.email}
              </p>
            )}
          </div>

          {/* Password Field */}
          <div className={styles.fieldGroup}>
            <label htmlFor="password" className={styles.label}>
              Password *
            </label>
            <div className={styles.inputWrapper}>
              <input
                type={showPassword ? 'text' : 'password'}
                id="password"
                name="password"
                value={formData.password}
                onChange={handleInputChange}
                className={`purple-input ${styles.inputWithIcon}`}
                placeholder="Create password"
                disabled={isSubmitting}
              />
              <button
                type="button"
                onClick={() => togglePasswordVisibility('password')}
                className={styles.toggleButton}
              >
                {showPassword ? '👁️' : '👁️‍🗨️'}
              </button>
            </div>
            {errors.password && (
              <p className={styles.errorText}>
                {errors.password}
              </p>
            )}
            <p className={styles.hintText}>
              8+ chars, uppercase, lowercase, number
            </p>
          </div>

          {/* Confirm Password Field */}
          <div className={styles.fieldGroupLast}>
            <label htmlFor="confirmPassword" className={styles.label}>
              Confirm Password *
            </label>
            <div className={styles.inputWrapper}>
              <input
                type={showConfirmPassword ? 'text' : 'password'}
                id="confirmPassword"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleInputChange}
                className={`purple-input ${styles.inputWithIcon}`}
                placeholder="Confirm password"
                disabled={isSubmitting}
              />
              <button
                type="button"
                onClick={() => togglePasswordVisibility('confirmPassword')}
                className={styles.toggleButton}
              >
                {showConfirmPassword ? '👁️' : '👁️‍🗨️'}
              </button>
            </div>
            {errors.confirmPassword && (
              <p className={styles.errorText}>
                {errors.confirmPassword}
              </p>
            )}
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            disabled={isSubmitting}
            className={`purple-button-primary ${styles.submitButton} ${
              isSubmitting ? 'opacity-50 cursor-not-allowed' : ''
            }`}
          >
            {isSubmitting ? 'Creating...' : 'Create Account'}
          </button>

          {/* Sign In Link */}
          <div className={styles.linkSection}>
            <p className={styles.linkText}>
              Already have an account?{' '}
              <button 
                type="button"
                onClick={onToggleMode}
                className={styles.link}
              >
                Sign In
              </button>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SignUp;