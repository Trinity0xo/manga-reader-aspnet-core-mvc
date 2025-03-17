function timeAgo(date) {
  let seconds = Math.floor((new Date() - new Date(date)) / 1000);

  if (seconds < 60) return "Vài giây trước";
  let minutes = Math.floor(seconds / 60);
  if (minutes < 60) return `${minutes} phút${minutes > 1 ? "s" : ""} trước`;
  let hours = Math.floor(minutes / 60);
  if (hours < 24) return `${hours} tiếng${hours > 1 ? "s" : ""} trước`;
  let days = Math.floor(hours / 24);
  if (days < 30) return `${days} day${days > 1 ? "s" : ""} trước`;
  let months = Math.floor(days / 30);
  if (months < 12) return `${months} tháng${months > 1 ? "s" : ""} trước`;
  let years = Math.floor(days / 365);
  return `${years} năm${years > 1 ? "s" : ""} trước`;
}

function updateTimeAgo() {
  $(".time-ago").each(function () {
    let time = $(this).attr("data-time");
    if (time) {
      $(this).text(timeAgo(time));
    }
  });
}

$(document).ready(function () {
  updateTimeAgo();
  setInterval(updateTimeAgo, 60000);
});
