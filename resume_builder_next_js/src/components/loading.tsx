export default function Loading() {
  return (
    <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
      <img
        src="/resource/loading.gif"
        alt="Loading..."
        width={300}
        height={300}
      />
    </div>
  );
}
