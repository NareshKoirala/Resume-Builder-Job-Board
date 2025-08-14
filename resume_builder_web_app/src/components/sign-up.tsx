'use client';

import React, { useState } from 'react';
import styles from '../css/auth.module.css';

interface SignUpFormData {
  email: string;
  password: string;
  confirmPassword: string;
}

interface SignUpErrors {
  email?: string;
  password?: string;
  confirmPassword?: string;
  general?: string;
}

interface SignUpProps {
  onToggleMode: () => void;
}

const SignUp: React.FC<SignUpProps> = ({ onToggleMode }) => {
  const [formData, setFormData] = useState<SignUpFormData>({
    email: '',
    password: '',
    confirmPassword: '',
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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);
    
    try {
      // TODO: Implement actual API call for sign up
      console.log('Sign up data:', { 
        email: formData.email, 
        password: formData.password 
      });
      
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Success - you might want to redirect or show success message
      alert('Account created successfully!');
      
      // Reset form
      setFormData({
        email: '',
        password: '',
        confirmPassword: '',
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