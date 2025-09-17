export default function Loading() {
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
        zIndex: 9999, // make sure it's on top
      }}
    >
      <img
        src="/resource/loading.gif"
        alt="Loading..."
        style={{ borderRadius: "12px" }}
      />
    </div>
  );
}
