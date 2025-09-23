interface LoadingProp {
  message?: string;
}

export default function Loading({ message = "Loading..." }: LoadingProp) {
  return (
    <div
      style={{
        position: "fixed",
        top: 0,
        left: 0,
        width: "100vw",
        height: "100vh",
        background: "rgba(0, 0, 0, 0.4)", // dark overlay
        backdropFilter: "blur(6px)", // glass blur effect
        WebkitBackdropFilter: "blur(6px)", // Safari support
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        zIndex: 9999,
      }}
    >
      {/* Inner wrapper for column layout */}
      <div
        style={{
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          gap: "12px", // spacing between gif and text
        }}
      >
        <img
          src="/resource/loading.gif"
          alt="Loading..."
          style={{ borderRadius: "12px" }}
        />
        <p className="text-lg font-semibold text-white">{message}</p>
      </div>
    </div>
  );
}
