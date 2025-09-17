'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';
import styles from '../css/auth.module.css';
import { emailFetch } from '@/app/api/supabase/dbFetch';

interface SignInFormData {
  email: string;
  password: string;
  rememberMe: boolean;
}

interface SignInErrors {
  email?: string;
  password?: string;
  general?: string;
}

interface SignInProps {
  onToggleMode: () => void;
}

const SignIn: React.FC<SignInProps> = ({ onToggleMode }) => {
  const router = useRouter();
  const [formData, setFormData] = useState<SignInFormData>({
    email: '',
    password: '',
    rememberMe: false,
  });

  const [errors, setErrors] = useState<SignInErrors>({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  const validateEmail = (email: string): boolean => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  };

  const validateForm = (): boolean => {
    const newErrors: SignInErrors = {};

    // Email validation
    if (!formData.email) {
      newErrors.email = 'Email is required';
    } else if (!validateEmail(formData.email)) {
      newErrors.email = 'Please enter a valid email address';
    }

    // Password validation
    if (!formData.password) {
      newErrors.password = 'Password is required';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));

    // Clear specific error when user starts typing
    if (errors[name as keyof SignInErrors]) {
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
      const response = await fetch('./api/signin', {
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
        setErrors({ general: result.message });
        return;
      }

      // Set user data in context or state management
      const userData = await emailFetch(formData.email);
      console.log('Fetched user data:', userData[0]);

      await fetch('/api/cookies/set', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          age: formData.rememberMe ? 60 * 60 * 24 * 30 : 60 * 60 * 2, // 30 days or 2 hours
          data: userData[0].public_id,
          id: 'publicId',
        }),
      });

      // Redirect to dashboard on successful sign-in
      router.push(`/dashboard`);
      
    } catch (error) {
      setErrors({ general: 'Failed to sign in. Please try again.' });
    } finally {
      setIsSubmitting(false);
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className={styles.authContainer}>
      <div className={styles.authForm}>
        <h2 className={styles.authTitle}>Welcome Back</h2>
        
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
                placeholder="Enter your password"
                disabled={isSubmitting}
              />
              <button
                type="button"
                onClick={togglePasswordVisibility}
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
            <a href="/forgot-password" className={styles.forgotPasswordLink}>
              Forgot password?
            </a>
          </div>

          {/* Remember Me Checkbox */}
          <div className={styles.checkboxWrapper}>
            <input
              type="checkbox"
              id="rememberMe"
              name="rememberMe"
              checked={formData.rememberMe}
              onChange={handleInputChange}
              className={styles.checkbox}
              disabled={isSubmitting}
            />
            <label htmlFor="rememberMe" className={styles.checkboxLabel}>
              Remember me for 30 days
            </label>
          </div>

          {/* Submit Button */}
          <button
            type="submit"
            disabled={isSubmitting}
            className={`purple-button-primary ${styles.submitButton} ${
              isSubmitting ? 'opacity-50 cursor-not-allowed' : ''
            }`}
          >
            {isSubmitting ? 'Signing In...' : 'Sign In'}
          </button>

          {/* Sign Up Link */}
          <div className={styles.linkSection}>
            <p className={styles.linkText}>
              Don't have an account?{' '}
              <button 
                type="button"
                onClick={onToggleMode}
                className={styles.link}
              >
                Create Account
              </button>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default SignIn;
