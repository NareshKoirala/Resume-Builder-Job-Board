import { useEffect, useState } from "react";
import { CheckCircle, XCircle, MinusCircle } from "lucide-react";

interface PopupProps {
  status: boolean | null;
  message?: string;
  onClose?: () => void; // new
}

const Popup: React.FC<PopupProps> = ({ status, message, onClose }) => {
  const [visible, setVisible] = useState(false);
  const [animateOut, setAnimateOut] = useState(false);

  useEffect(() => {
    if (status !== undefined) {
      setVisible(true);

      const timerOut = setTimeout(() => setAnimateOut(true), 500);
      const timerHide = setTimeout(() => {
        setVisible(false);
        setAnimateOut(false);
        onClose?.(); // reset in parent
      }, 1000);

      return () => {
        clearTimeout(timerOut);
        clearTimeout(timerHide);
      };
    }
  }, [status]);

  if (!visible) return null;

  let bgColor = "bg-gray-800";
  let Icon = MinusCircle;
  if (status === true) {
    bgColor = "bg-green-600";
    Icon = CheckCircle;
  } else if (status === false) {
    bgColor = "bg-red-600";
    Icon = XCircle;
  }

  return (
    <div
      className={`
        fixed bottom-6 right-6 z-50 min-w-[260px] max-w-sm px-5 py-4 rounded-2xl
        shadow-2xl flex items-start gap-3
        ${bgColor}
        transition-all duration-500 ease-in-out
        ${animateOut ? "translate-x-28 opacity-0" : "translate-x-0 opacity-100"}
      `}
    >
      <Icon className="w-6 h-6 flex-shrink-0 text-white drop-shadow-md" />
      <p className="text-base font-semibold leading-snug text-white drop-shadow-sm">
        {message || "Notification"}
      </p>
    </div>
  );
};

export default Popup;
