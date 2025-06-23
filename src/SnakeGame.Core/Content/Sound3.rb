use_synth :pulse
notes = [60, 64, 67, 72, 76]
notes.each_with_index do |n, i|
  play n, release: 0.1 + i*0.05, amp: 0.4 + i*0.1
  sleep 0.1
end
