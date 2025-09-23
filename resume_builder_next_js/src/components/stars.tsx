'use client';

import React, { useEffect, useState } from 'react';
import '../css/animations.css';

interface Star {
  id: number;
  x: number;
  y: number;
  size: number;
  opacity: number;
  animationClass: string;
  animationDelay: string;
  radius: number;
  angle: number;
  centerX: number;
  centerY: number;
  speed: number;
}

interface StarsProps {
  count?: number;
  minSize?: number;
  maxSize?: number;
  className?: string;
}

const Stars: React.FC<StarsProps> = ({
  count = 150,
  minSize = 0.5,
  maxSize = 2,
  className = ''
}) => {
  const [stars, setStars] = useState<Star[]>([]);

  useEffect(() => {
    const generateStars = () => {
      const newStars: Star[] = [];
      const animationClasses = ['twinkle', 'pulse', 'fade-in', 'scale-in'];
      const delayClasses = ['anim-delay-100', 'anim-delay-200', 'anim-delay-300'];
      
      for (let i = 0; i < count; i++) {
        const randomAnimation = animationClasses[Math.floor(Math.random() * animationClasses.length)];
        const randomDelay = delayClasses[Math.floor(Math.random() * delayClasses.length)];
        
        // Galaxy rotation properties - all stars rotate around screen center
        const centerX = 50; // Center of screen (X)
        const centerY = 50; // Center of screen (Y)
        const radius = Math.random() * 40 + 5; // Radius between 5-45 (distance from center)
        const angle = Math.random() * 360; // Starting angle
        const speed = 0.35; // Same speed for all stars - galaxy-like rotation

        // Calculate initial position
        const x = centerX + (radius * Math.cos(angle * Math.PI / 180));
        const y = centerY + (radius * Math.sin(angle * Math.PI / 180));
        
        newStars.push({
          id: i,
          x: Math.max(0, Math.min(100, x)), // Clamp to screen bounds
          y: Math.max(0, Math.min(100, y)), // Clamp to screen bounds
          size: Math.random() * (maxSize - minSize) + minSize,
          opacity: Math.random() * 0.8 + 0.1, // Between 0.1 and 0.9
          animationClass: randomAnimation,
          animationDelay: randomDelay,
          radius: radius,
          angle: angle,
          centerX: centerX,
          centerY: centerY,
          speed: speed
        });
      }
      
      setStars(newStars);
    };

    generateStars();
  }, [count, minSize, maxSize]);

  // Animation loop for circular motion
  useEffect(() => {
    const animationInterval = setInterval(() => {
      setStars(prevStars => 
        prevStars.map(star => {
          // Update angle for clockwise motion
          const newAngle = star.angle + star.speed;
          
          // Calculate new position
          const newX = star.centerX + (star.radius * Math.cos(newAngle * Math.PI / 180));
          const newY = star.centerY + (star.radius * Math.sin(newAngle * Math.PI / 180));
          
          return {
            ...star,
            angle: newAngle % 360, // Keep angle within 0-360 degrees
            x: Math.max(0, Math.min(100, newX)), // Clamp to screen bounds
            y: Math.max(0, Math.min(100, newY))  // Clamp to screen bounds
          };
        })
      );
    }, 80); // Update every 80ms for smooth, noticeable galaxy rotation

    return () => clearInterval(animationInterval);
  }, [stars.length]); // Re-run when stars array changes

  return (
    <div className={`fixed inset-0 pointer-events-none overflow-hidden ${className}`}>
      {stars.map((star) => (
        <div
          key={star.id}
          className={`absolute ${star.animationClass} ${star.animationDelay} anim-fill-both`}
          style={{
            left: `${star.x}%`,
            top: `${star.y}%`,
            width: `${star.size}px`,
            height: `${star.size}px`,
            opacity: star.opacity
          }}
        >
          {/* Star shape using CSS */}
          <div
            className="relative bg-white rounded-full shadow-sm hover-glow"
            style={{
              width: '100%',
              height: '100%',
              boxShadow: `0 0 ${star.size * 2}px rgba(255, 255, 255, 0.5)`
            }}
          >
            {/* Cross pattern to make it look more star-like */}
            <div
              className="absolute bg-white"
              style={{
                width: '1px',
                height: `${star.size * 2}px`,
                left: '50%',
                top: '50%',
                transform: 'translate(-50%, -50%)',
                opacity: 0.8
              }}
            />
            <div
              className="absolute bg-white"
              style={{
                width: `${star.size * 2}px`,
                height: '1px',
                left: '50%',
                top: '50%',
                transform: 'translate(-50%, -50%)',
                opacity: 0.8
              }}
            />
          </div>
        </div>
      ))}
    </div>
  );
};

export default Stars;